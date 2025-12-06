using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebportSystem.Inventory.Application.Data;
using WebportSystem.Inventory.Infrastructure.Common;
using WebportSystem.Inventory.Infrastructure.Database;

namespace WebportSystem.Inventory.Infrastructure;

public static class InventoryModule
{
    public static IServiceCollection AddInventoryModule(
        this IServiceCollection services,
        IConfiguration configuration,
        string inventoryDatabaseString)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddInfrastructure(inventoryDatabaseString);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        string inventoryDatabaseString)
    {
        services.AddDbContext<IInventoryDbContext, InventoryDbContext>((sp, options) =>
        {
            options.UseNpgsql(inventoryDatabaseString, npgsqlOptionsAction =>
            {
                npgsqlOptionsAction.EnableRetryOnFailure(
                        maxRetryCount: 1,
                        maxRetryDelay: TimeSpan.FromSeconds(2),
                        errorCodesToAdd: null);

                npgsqlOptionsAction.MigrationsHistoryTable(HistoryRepository.DefaultTableName, InventoryConstants.Schema);
            })
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(
                sp.GetRequiredService<AuditableEntityInterceptor>(),
                sp.GetRequiredService<InsertOutboxMessagesInterceptor>());
        });
    }
}