using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Presentation.Endpoints;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Application.Interfaces;
using WebportSystem.Identity.Domain.Users;
using WebportSystem.Identity.Infrastructure.Database;
using WebportSystem.Identity.Infrastructure.Outbox;
using WebportSystem.Identity.Infrastructure.Services;

namespace WebportSystem.Identity.Infrastructure;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        IConfiguration configuration,
        string identityDatabaseString)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDomainEventHandlers();

        services.AddInfrastructure(identityDatabaseString);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        string systemDatabaseString)
    {

        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddScoped<ITokenService, TokenService>();

        services.AddDbContext<IUsersDbContext, UsersDbContext>((sp, options) =>
        {
            options.UseNpgsql(systemDatabaseString, npgsqlOptionsAction =>
            {
                npgsqlOptionsAction.EnableRetryOnFailure(
                        maxRetryCount: 1,
                        maxRetryDelay: TimeSpan.FromSeconds(2),
                        errorCodesToAdd: null);

                npgsqlOptionsAction.MigrationsHistoryTable(HistoryRepository.DefaultTableName, ModuleConstants.Schema);
            })
            .UseSnakeCaseNamingConvention()
            ;
        });
    }

    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = [.. Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventDispatcher)))];

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
}