using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebportSystem.Identity.Domain.Roles;
using WebportSystem.Identity.Domain.Users;
using WebportSystem.Identity.Infrastructure.Database;

namespace WebportSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MigrationController(
    UsersDbContext usersDb,
    //InventoryDbContext inventoryDb,
    UserManager<UserM> userManager,
    RoleManager<RoleM> roleManager) : ControllerBase
{

    // 🔹 Seed Identity
    [HttpPost("seed-identity")]
    public async Task<IActionResult> SeedIdentity()
    {
        await IdentitySeedService.SeedAsync(usersDb, userManager, roleManager);
        return Ok(new { Message = "Identity data seeded successfully", Time = DateTime.UtcNow });
    }

    // 🔹 Seed Inventory
    [HttpPost("seed-inventory")]
    public async Task<IActionResult> SeedInventory()
    {
        // await InventoryDataSeeder.SeedAsync(inventoryDb);
        return Ok(new { Message = "Inventory data seeded successfully", Time = DateTime.UtcNow });
    }

    // 🔹 Seed ALL (recommended shortcut)
    [HttpPost("seed-all")]
    public async Task<IActionResult> SeedAll()
    {
        await IdentitySeedService.SeedAsync(usersDb, userManager, roleManager);
        // await InventoryDataSeeder.SeedAsync(inventoryDb);

        return Ok(new { Message = "Seeding completed", Time = DateTime.UtcNow });
    }
}