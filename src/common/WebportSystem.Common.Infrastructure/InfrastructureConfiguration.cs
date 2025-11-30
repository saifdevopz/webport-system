using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using WebportSystem.Common.Application.Database;
using WebportSystem.Common.Infrastructure.Authentication;
using WebportSystem.Common.Infrastructure.Clock;
using WebportSystem.Common.Infrastructure.Database;
using WebportSystem.Common.Infrastructure.Interceptors;

namespace WebportSystem.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddCommonInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthenticationInternal();

        // DateTime Provider
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        // EF Core Interceptors
        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();
        services.TryAddSingleton<AuditableEntityInterceptor>();

        // Database
        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        // Quartz
        services.AddQuartz(configurator =>
        {
            Guid scheduler = Guid.NewGuid();
            configurator.SchedulerId = $"default-id-{scheduler}";
            configurator.SchedulerName = $"default-name-{scheduler}";
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}
