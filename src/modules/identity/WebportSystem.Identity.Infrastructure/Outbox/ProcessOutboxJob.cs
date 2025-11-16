using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System.Data;
using System.Data.Common;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Application.Database;
using WebportSystem.Common.Domain.Abstractions;
using WebportSystem.Common.Infrastructure.Clock;
using WebportSystem.Common.Infrastructure.Outbox;
using WebportSystem.Common.Infrastructure.Serialization;
using WebportSystem.Identity.Application;

namespace WebportSystem.Identity.Infrastructure.Outbox;

[DisallowConcurrentExecution] // To allow only one instance of a background job.
internal sealed class ProcessOutboxJob(
        IDbConnectionFactory _dbConnectionFactory,
        IServiceScopeFactory serviceScopeFactory,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxJob> logger) : IJob
{
    private const string ModuleName = ModuleConstants.ModuleName;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{Module} - Beginning to process outbox messages", ModuleName);

        await using DbConnection connection = await _dbConnectionFactory.OpenIdentityConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();

        // Get unprocessed outbox messages from database.
        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                        outboxMessage.Content,
                        SerializerSettings.Instance)!;

                using IServiceScope scope = serviceScopeFactory.CreateScope();

                IEnumerable<IDomainEventDispatcher> handlers = DomainEventHandlersFactory.GetHandlers(
                        domainEvent.GetType(),
                        scope.ServiceProvider,
                        AssemblyReference.Assembly);

                foreach (IDomainEventDispatcher domainEventHandler in handlers)
                {
                    await domainEventHandler.Handle(domainEvent, context.CancellationToken);
                }
            }
            catch (TaskSchedulerException caughtException)
            {
                logger.LogError(
                        caughtException,
                        "{Module} - Exception while processing outbox message {MessageId}",
                        ModuleName,
                        outboxMessage.Id);

                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        await transaction.CommitAsync();

        logger.LogInformation("{Module} - Completed processing outbox messages", ModuleName);
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
            IDbConnection connection,
            IDbTransaction transaction)
    {

        string sql =
            $"""
             SELECT
                id AS {nameof(OutboxMessageResponse.Id)},
                content AS {nameof(OutboxMessageResponse.Content)}
             FROM {ModuleConstants.Schema}.outbox_messages
             WHERE processed_on_utc IS NULL
             ORDER BY occurred_on_utc
             LIMIT {outboxOptions.Value.BatchSize}
             FOR UPDATE
             """;

        IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(
                sql,
                transaction: transaction);

        return [.. outboxMessages];
    }

    private async Task UpdateOutboxMessageAsync(
            IDbConnection connection,
            IDbTransaction transaction,
            OutboxMessageResponse outboxMessage,
            Exception? exception)
    {

        const string sql =
            $"""
            UPDATE {ModuleConstants.Schema}.outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            WHERE id = @Id
            """;

        await connection.ExecuteAsync(
                sql,
                new
                {
                    outboxMessage.Id,
                    ProcessedOnUtc = dateTimeProvider.Now,
                    Error = exception?.ToString()
                },
                transaction: transaction);
    }

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}
