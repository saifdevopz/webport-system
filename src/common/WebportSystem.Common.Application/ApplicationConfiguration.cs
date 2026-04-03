using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Application.Behaviors;

namespace WebportSystem.Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddCommonApplication(
    this IServiceCollection services,
        Assembly[] moduleAssemblies,
        (Type ContextType, Assembly EntityAssembly)[] moduleContexts)
    {
        RegisterQueryHandlers(services, moduleAssemblies);
        RegisterCommandHandlers(services, moduleAssemblies);
        RegisterGenericHandlers(services, moduleContexts);
        RegisterPipelineBehaviors(services);

        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);

        return services;
    }

    private static void RegisterCommandHandlers(IServiceCollection services, Assembly[] moduleAssemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(moduleAssemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }

    private static void RegisterQueryHandlers(IServiceCollection services, Assembly[] moduleAssemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(moduleAssemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }

    private static void RegisterPipelineBehaviors(IServiceCollection services)
    {
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));

        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandBaseHandler<>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
    }

    private static void RegisterGenericHandlers(
    IServiceCollection services,
    (Type ContextType, Assembly EntityAssembly)[] moduleContexts)
    {
        foreach (var (contextType, entityAssembly) in moduleContexts)
        {
            var entityTypes = entityAssembly
                .GetTypes()
                .Where(t => typeof(AggregateRoot).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (var entity in entityTypes)
            {
                var deleteCommand = typeof(GenericDeleteCommand<>).MakeGenericType(entity);
                services.AddScoped(
                    typeof(ICommandHandler<>).MakeGenericType(deleteCommand),
                    typeof(GenericDeleteCommandHandler<,>).MakeGenericType(entity, contextType));
            }
        }
    }
}
