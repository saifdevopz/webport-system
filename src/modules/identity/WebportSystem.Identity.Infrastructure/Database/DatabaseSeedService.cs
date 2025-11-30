using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebportSystem.Identity.Domain.Roles;
using WebportSystem.Identity.Domain.Tenants;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Infrastructure.Database;

public static class DatabaseSeedService
{
    public static async Task SeedAsync(UsersDbContext dbContext, UserManager<UserM> userManager,
        RoleManager<RoleM> roleManager)
    {
        await dbContext.Database.MigrateAsync();

        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        // Seed Tenants

        TenantM[] tenants =
        [
            TenantM.Create("Administrator"),
            TenantM.Create("Customer1"),
            TenantM.Create("Customer2"),
        ];

        await dbContext.Tenants.AddRangeAsync(tenants);
        await dbContext.SaveChangesAsync();

        // Seed Roles

        var adminRole = new RoleM { Name = "Admin" };
        var userRole = new RoleM { Name = "User" };

        await roleManager.CreateAsync(adminRole);
        await roleManager.CreateAsync(userRole);

        // Seed Users

        var adminUser = new UserM
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = tenants[0].TenantId,
            Email = "admin@gmail.com",
            UserName = "Administrator"
        };

        await userManager.CreateAsync(adminUser, "12345678");
        await userManager.AddToRoleAsync(adminUser, "Admin");

        var user1 = new UserM
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = tenants[1].TenantId,
            Email = "customer1@gmail.com",
            UserName = "Customer1"
        };

        await userManager.CreateAsync(user1, "12345678");
        await userManager.AddToRoleAsync(user1, "User");

        var user2 = new UserM
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = tenants[2].TenantId,
            Email = "customer2@gmail.com",
            UserName = "Customer2"
        };

        await userManager.CreateAsync(user2, "12345678");
        await userManager.AddToRoleAsync(user2, "User");

        // Seed Role Claims

        await roleManager.AddClaimAsync(adminRole, new Claim("permission", "identity:global"));
        await roleManager.AddClaimAsync(adminRole, new Claim("permission", "inventory:global"));
        await roleManager.AddClaimAsync(userRole, new Claim("permission", "inventory:global"));
    }
}
