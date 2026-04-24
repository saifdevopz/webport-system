using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Identity.Domain.Roles;
using WebportSystem.Identity.Domain.Users;
using WebportSystem.Identity.Infrastructure.Database;
using WebportSystem.Inventory.Infrastructure.Database;

namespace WebportSystem.Api.Extensions;

internal static class DatabaseInitializer
{

    public static async Task InitializeDatabases(this IApplicationBuilder app)
    {
        await app.ApplyIdentityMigrations();
        await app.ApplyIdentityDataSeeder();

        await app.ApplyInventoryMigrations();
        await app.ApplyInventoryDataSeeder();
    }

    public static async Task ApplyIdentityMigrations(this IApplicationBuilder app)
    {
        app.ApplyCustomMigration<UsersDbContext>(null);
        await Task.CompletedTask;
    }

    public static async Task ApplyInventoryMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        var tenants = await usersDbContext.Tenants
            .AsNoTracking()
            .ToListAsync();

        foreach (var tenant in tenants)
        {
            if (string.IsNullOrWhiteSpace(tenant.DatabaseConnectionString))
            {
                Console.WriteLine($"Skipping tenant '{tenant.TenantName}' - missing connection string.");
                continue;
            }

            ApplyCustomMigration<InventoryDbContext>(app, tenant.DatabaseConnectionString);
        }
    }

    private static void ApplyCustomMigration<TDbContext>(this IApplicationBuilder app, string? connectionString)
        where TDbContext : DbContext
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        try
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                context.Database.SetConnectionString(connectionString);
            }


            if (context.Database.GetPendingMigrations().Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Applying Migrations for '{nameof(TDbContext) ?? "System Database"}'.");
                Console.ResetColor();
                context.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Migration failed for '{connectionString}': {ex.Message}");
            Console.ResetColor();
        }
    }

    public static async Task ApplyIdentityDataSeeder(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserM>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleM>>();

        await IdentitySeedService.SeedAsync(dbContext, userManager, roleManager);
    }

    public static async Task ApplyInventoryDataSeeder(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        var tenants = await usersDbContext.Tenants
            .AsNoTracking()
            .ToListAsync();

        foreach (var tenant in tenants)
        {
            if (string.IsNullOrWhiteSpace(tenant.DatabaseConnectionString))
            {
                Console.WriteLine($"Skipping tenant '{tenant.TenantName}' - missing connection string.");
                continue;
            }

            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
                optionsBuilder.UseNpgsql(tenant.DatabaseConnectionString);
                optionsBuilder.UseSnakeCaseNamingConvention();
                using var context = new InventoryDbContext(optionsBuilder.Options);

                await InventoryDataSeeder.SeedAsync(context, tenant.TenantName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding failed for '{tenant.TenantName}': {ex.Message}");
            }
        }
    }
}
