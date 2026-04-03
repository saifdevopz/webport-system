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
        await app.ApplyInventoryMigrations();

        await app.ApplyIdentityDataSeeder();
        await app.ApplyInventoryDataSeeder();
    }

    public static async Task ApplyIdentityMigrations(this IApplicationBuilder app)
    {
        app.ApplyCustomMigration<UsersDbContext>(null);
        await Task.CompletedTask;
    }

    public static async Task ApplyInventoryMigrations(this IApplicationBuilder app)
    {
        app.ApplyCustomMigration<InventoryDbContext>(null);
        await Task.CompletedTask;
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

    public static async Task ApplyInventoryDataSeeder(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        await InventoryDataSeeder.SeedAsync(context);
    }
}
