using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;
using WebportSystem.Common.Application.Abstractions;

namespace WebportSystem.Common.Infrastructure.Outbox;

public static class DomainEventHandlersFactory
{
    private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

    public static IEnumerable<IDomainEventDispatcher> GetHandlers(
            Type type,
            IServiceProvider serviceProvider,
            Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(assembly);

        Type[] domainEventHandlerTypes = HandlersDictionary.GetOrAdd(
                $"{assembly.GetName().Name}{type.Name}",
                _ =>
                {
                    Type[] domainEventHandlerTypes = [.. assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IDomainEventDispatcher<>).MakeGenericType(type)))];

                    return domainEventHandlerTypes;
                });

        List<IDomainEventDispatcher> handlers = [];
        foreach (Type domainEventHandlerType in domainEventHandlerTypes)
        {
            object domainEventHandler = serviceProvider.GetRequiredService(domainEventHandlerType);

            handlers.Add((domainEventHandler as IDomainEventDispatcher)!);
        }

        return handlers;
    }
}