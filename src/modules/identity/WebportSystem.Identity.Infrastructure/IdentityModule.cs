using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Application.Authorization;
using WebportSystem.Common.Infrastructure.Database;
using WebportSystem.Common.Infrastructure.Interceptors;
using WebportSystem.Common.Presentation.Endpoints;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Application.Interfaces;
using WebportSystem.Identity.Domain.Roles;
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


#pragma warning disable S125 // Sections of code should not be commented out
        //RegisterGenericHandlers<UsersDbContext>(services, typeof(RoleM).Assembly);

        return services;
#pragma warning restore S125 // Sections of code should not be commented out
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        string systemDatabaseString)
    {

        services.AddIdentityCore<UserM>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
            .AddRoles<RoleM>()
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddScoped<TenantProvider>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPermissionService, PermissionService>();

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
            .AddInterceptors(
                sp.GetRequiredService<AuditableEntityInterceptor>(),
                sp.GetRequiredService<InsertOutboxMessagesInterceptor>());
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

    //private static void RegisterGenericHandlers<TContext>(
    //    IServiceCollection services,
    //    Assembly entityAssembly)
    //    where TContext : DbContext

#pragma warning disable S125 // Sections of code should not be commented out
    //{
    //    var entityTypes = entityAssembly
    //        .GetTypes()
    //        .Where(t => typeof(ISimpleEntity).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

    //    foreach (var entity in entityTypes)
    //    {
    //        var deleteCommand = typeof(GenericDeleteCommand<>).MakeGenericType(entity);
    //        services.AddScoped(
    //            typeof(ICommandHandler<>).MakeGenericType(deleteCommand),
    //            typeof(GenericDeleteCommandHandler<,>).MakeGenericType(entity, typeof(TContext)));
    //    }
    //}
}
#pragma warning restore S125 // Sections of code should not be commented out