using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebportSystem.Identity.Domain.Roles;
using WebportSystem.Identity.Domain.Tenants;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Infrastructure.Database;

public static class IdentitySeedService
{
    public static async Task SeedAsync(
        UsersDbContext dbContext,
        UserManager<UserM> userManager,
        RoleManager<RoleM> roleManager)
    {
        if (await dbContext.Tenants.AnyAsync()) return;

        var tenants = await SeedTenantsAsync(dbContext);
        var (adminRole, userRole) = await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager, tenants);
        await SeedRoleClaimsAsync(roleManager, adminRole, userRole);
    }

    private static async Task<TenantM[]> SeedTenantsAsync(UsersDbContext dbContext)
    {
        TenantM[] tenants =
        [
            TenantM.Create("Administrator"),
            TenantM.Create("Customer1"),
            TenantM.Create("Customer2"),
            TenantM.Create("Customer3"),
        ];

        await dbContext.Tenants.AddRangeAsync(tenants);
        await dbContext.SaveChangesAsync();

        return tenants;
    }

    private static async Task<(RoleM adminRole, RoleM userRole)> SeedRolesAsync(
        RoleManager<RoleM> roleManager)
    {
        var adminRole = new RoleM { Name = "Admin" };
        var userRole = new RoleM { Name = "User" };

        await roleManager.CreateAsync(adminRole);
        await roleManager.CreateAsync(userRole);

        return (adminRole, userRole);
    }

    private static async Task SeedUsersAsync(
        UserManager<UserM> userManager,
        TenantM[] tenants)
    {
        var adminUser = new UserM
        {
            TenantId = tenants[0].TenantId,
            Email = "admin@gmail.com",
            UserName = "Administrator"
        };

        await userManager.CreateAsync(adminUser, "12345678");
        await userManager.AddToRoleAsync(adminUser, "Admin");

        var user1 = new UserM
        {
            TenantId = tenants[1].TenantId,
            Email = "customer1@gmail.com",
            UserName = "Customer1"
        };

        await userManager.CreateAsync(user1, "12345678");
        await userManager.AddToRoleAsync(user1, "User");

        var user2 = new UserM
        {
            TenantId = tenants[2].TenantId,
            Email = "customer2@gmail.com",
            UserName = "Customer2"
        };

        await userManager.CreateAsync(user2, "12345678");
        await userManager.AddToRoleAsync(user2, "User");

        var user3 = new UserM
        {
            TenantId = tenants[3].TenantId,
            Email = "customer3@gmail.com",
            UserName = "Customer3"
        };

        await userManager.CreateAsync(user3, "12345678");
        await userManager.AddToRoleAsync(user3, "User");
    }

    private static async Task SeedRoleClaimsAsync(
        RoleManager<RoleM> roleManager,
        RoleM adminRole,
        RoleM userRole)
    {
        await roleManager.AddClaimAsync(adminRole, new Claim("permission", "identity:global"));
        await roleManager.AddClaimAsync(adminRole, new Claim("permission", "inventory:global"));
        await roleManager.AddClaimAsync(userRole, new Claim("permission", "inventory:global"));
    }
}
