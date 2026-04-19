using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Identity.Domain.Roles;
using WebportSystem.Identity.Domain.Tenants;
using WebportSystem.Identity.Domain.Users;
using WebportSystem.Identity.Infrastructure.Database;
using WebportSystem.Inventory.Infrastructure.Database;

namespace WebportSystem.Api.Extensions;

internal static class DatabaseInitializer
{
    public static async Task InitializeDatabases(this IApplicationBuilder app)
#pragma warning disable S125 // Sections of code should not be commented out
    {
        await app.ApplyIdentityMigrations();
        await app.ApplyIdentityDataSeeder();

        //await app.ApplyInventoryMigrations();

        //await app.ApplyInventoryDataSeeder();
    }
#pragma warning restore S125 // Sections of code should not be commented out

    public static async Task ApplyIdentityMigrations(this IApplicationBuilder app)
    {
        app.ApplyCustomMigration<UsersDbContext>(null);
        await Task.CompletedTask;
    }

    public static async Task ApplyInventoryMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        var tenants = await usersDbContext.Tenants
            .AsNoTracking()
            .ToListAsync();

        foreach (var tenant in tenants)
        {
            var tenantConnectionString = tenant.DatabaseConnectionString;

            if (string.IsNullOrWhiteSpace(tenantConnectionString))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Skipping tenant '{tenant.TenantName}' - connection string missing.");
                Console.ResetColor();

                continue;
            }

            app.ApplyCustomMigration<InventoryDbContext>(tenantConnectionString);
            await ApplyInventoryDataSeeder(app, tenant);
        }
    }

    private static void ApplyCustomMigration<TDbContext>(this IApplicationBuilder app, string? connectionString)
        where TDbContext : DbContext
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        if (connectionString != null)
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

    public static async Task ApplyIdentityDataSeeder(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserM>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleM>>();

        await IdentitySeedService.SeedAsync(dbContext, userManager, roleManager);
    }

    public static async Task ApplyInventoryDataSeeder(this IApplicationBuilder app, TenantM tenant)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        await InventoryDataSeeder.SeedAsync(context, tenant.TenantName);
    }
}
