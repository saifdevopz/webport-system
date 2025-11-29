using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Infrastructure.Database;

public static class DatabaseSeedService
{
    public static async Task SeedAsync(UsersDbContext dbContext, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await dbContext.Database.MigrateAsync();

        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        var adminRole = new IdentityRole { Name = "Admin" };
        var userRole = new IdentityRole { Name = "User" };

        await roleManager.CreateAsync(adminRole);
        await roleManager.CreateAsync(userRole);

        await roleManager.AddClaimAsync(adminRole, new Claim("admin:create", "true"));
        await roleManager.AddClaimAsync(adminRole, new Claim("admin:update", "true"));
        await roleManager.AddClaimAsync(adminRole, new Claim("users:create", "true"));
        await roleManager.AddClaimAsync(adminRole, new Claim("users:update", "true"));

        var adminUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin@test.com",
            UserName = "admin@test.com"
        };

        await userManager.CreateAsync(adminUser, "12345678");
        await userManager.AddToRoleAsync(adminUser, "Admin");

        var user1 = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "user@gmail.com",
            UserName = "user@gmail.com"
        };

        await userManager.CreateAsync(user1, "12345678");
        await userManager.AddToRoleAsync(user1, "User");
    }
}
