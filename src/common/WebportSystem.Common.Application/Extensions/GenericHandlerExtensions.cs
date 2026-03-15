using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebportSystem.Common.Application.Abstractions;

namespace WebportSystem.Common.Application.Extensions;

public static class GenericHandlerExtensions
{
    public static IServiceCollection RegisterGenericHandlers<TContext>(
        this IServiceCollection services,
        Assembly entityAssembly)
        where TContext : DbContext
    {
        var entityTypes = entityAssembly
            .GetTypes()
            .Where(t => typeof(ISimpleEntity).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var entity in entityTypes)
        {
            var deleteCommand = typeof(GenericDeleteCommand<>).MakeGenericType(entity);
            services.AddScoped(
                typeof(ICommandHandler<>).MakeGenericType(deleteCommand),
                typeof(GenericDeleteCommandHandler<,>).MakeGenericType(entity, typeof(TContext)));
        }

        return services;
    }
}
