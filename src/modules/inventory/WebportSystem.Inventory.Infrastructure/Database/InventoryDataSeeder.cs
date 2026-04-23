using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;
using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Infrastructure.Database;

public static class InventoryDataSeeder
{
    public static async Task SeedAsync(InventoryDbContext context, string tenantName)
    {
        try
        {            
            await SeedBusinessProfilesAsync(context, tenantName);
            await SeedCustomersAsync(context, tenantName);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Seeding failed for {tenantName}: {ex}");
            throw;
        }
    }

    private static async Task SeedBusinessProfilesAsync(InventoryDbContext context, string tenantName)
    {
        var exists = await context.BusinessProfiles.AnyAsync();

        if (exists) return;

        var businessProfile = new BusinessProfileM(
            businessName: $"{tenantName} Business",
            email: $"info@{tenantName.ToLower()}.com",
            phone: "080 000 0000",
            addressLine1: "123 Default Street",
            city: "Durban",
            province: "KwaZulu-Natal",
            postalCode: "4001",
            country: "South Africa",
            bankName: "Standard Bank",
            accountNumber: "0000000000",
            branchCode: "051001"
        );

        await context.BusinessProfiles.AddAsync(businessProfile);        
    }

    private static async Task SeedCustomersAsync(InventoryDbContext context, string tenantName)
    {
        if (await context.Customers.AnyAsync())
            return;

        var customers = new[]
        {
            new CustomerM(
                name: "John Doe",
                email: "john@demo.com",
                phone: "082 123 4567",
                addressLine1: "12 Main Road",
                city: "Durban",
                province: "KwaZulu-Natal",
                postalCode: "4001",
                companyName: $"{tenantName} Ltd"),

            new CustomerM(
                name: "Jane Smith",
                email: "jane@demo.com",
                phone: "083 987 6543",
                addressLine1: "45 Market Street",
                city: "Johannesburg",
                province: "Gauteng",
                postalCode: "2000",
                companyName: $"{tenantName} Ltd")
        };

        await context.Customers.AddRangeAsync(customers);        
    }
}