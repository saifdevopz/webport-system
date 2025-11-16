using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using WebportSystem.Common.Domain.Abstractions;
using WebportSystem.Common.Infrastructure.Outbox;
using WebportSystem.Common.Infrastructure.Serialization;

namespace WebportSystem.Common.Infrastructure.Interceptors;

public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);

        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void InsertOutboxMessages(DbContext context)
    {
        List<OutboxMessage> outboxMessages = [.. context
                .ChangeTracker
                .Entries<BaseEntity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;

                    entity.ClearDomainEvents();

                    return domainEvents;
                })
                .Select(domainEvent => new OutboxMessage
                {
                    Id = domainEvent.Id,
                    Type = domainEvent.GetType().Name,
                    Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings.Instance),
                    OccurredOnUtc = domainEvent.OccurredOnUtc
                })];

        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}
