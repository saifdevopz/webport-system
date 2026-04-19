using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebportSystem.Identity.Domain.Platform;
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

        await SeedPlatformUserAsync(dbContext);
        var tenants = await SeedTenantsAsync(dbContext);
        var (adminRole, userRole) = await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager, tenants);
        await SeedRoleClaimsAsync(roleManager, adminRole, userRole);
    }

    private static async Task SeedPlatformUserAsync(UsersDbContext dbContext)
    {
        if (await dbContext.PlatformUsers.AnyAsync()) return;

        PlatformUserM[] platformUsers =
        [
            PlatformUserM.Create("admin@gmail.com", "12345678", "admin"),
        ];

        await dbContext.PlatformUsers.AddRangeAsync(platformUsers);
        await dbContext.SaveChangesAsync();
    }

    private static async Task<TenantM[]> SeedTenantsAsync(UsersDbContext dbContext)
    {
        TenantM[] tenants =
        [
            TenantM.Create("Customer1", "customer1-db"),
            TenantM.Create("Customer2", "customer2-db"),
            TenantM.Create("Customer3", "customer3-db"),
        ];

        await dbContext.Tenants.AddRangeAsync(tenants);
        await dbContext.SaveChangesAsync();

        return tenants;
    }

    private static async Task<(RoleM adminRole, RoleM userRole)> SeedRolesAsync(
        RoleManager<RoleM> roleManager)
    {
        var superAdminRole = new RoleM { Name = "SuperAdmin" };
        var adminRole = new RoleM { Name = "Admin" };
        var userRole = new RoleM { Name = "User" };

        await roleManager.CreateAsync(superAdminRole);
        await roleManager.CreateAsync(adminRole);
        await roleManager.CreateAsync(userRole);

        return (adminRole, userRole);
    }

    private static async Task SeedUsersAsync(
        UserManager<UserM> userManager,
        TenantM[] tenants)
    {
        var superUser = new UserM
        {
            TenantId = null,
            Email = "admin@gmail.com",
            UserName = "SuperAdmin"
        };

        await userManager.CreateAsync(superUser, "12345678");
        await userManager.AddToRoleAsync(superUser, "SuperAdmin");

        var user1 = new UserM
        {
            TenantId = tenants[0].TenantId,
            Email = "customer1@gmail.com",
            UserName = "Customer1"
        };

        await userManager.CreateAsync(user1, "12345678");
        await userManager.AddToRoleAsync(user1, "User");

        var user2 = new UserM
        {
            TenantId = tenants[1].TenantId,
            Email = "customer2@gmail.com",
            UserName = "Customer2"
        };

        await userManager.CreateAsync(user2, "12345678");
        await userManager.AddToRoleAsync(user2, "User");

        var user3 = new UserM
        {
            TenantId = tenants[2].TenantId,
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
