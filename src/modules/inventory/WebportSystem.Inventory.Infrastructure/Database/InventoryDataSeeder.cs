using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Infrastructure.Database;

public static class InventoryDataSeeder
{
    public static async Task SeedAsync(InventoryDbContext context, string tenantName)
    {
        await SeedBusinessProfilesAsync(context, tenantName);
        await SeedCustomersAsync(context, tenantName);
    }

    private static async Task SeedBusinessProfilesAsync(InventoryDbContext context, string tenantName)
    {
        if (await context.BusinessProfiles.AnyAsync()) return;

        BusinessProfileM[] businessProfiles = tenantName switch
        {
            "Demo" =>
            [
                new BusinessProfileM(
                    businessName: "Demo Store",
                    email: "demo@store.com",
                    phone: "080 000 0000",
                    addressLine1: "1 Demo Street",
                    city: "Cape Town",
                    province: "Western Cape",
                    postalCode: "8001",
                    country: "South Africa",
                    bankName: "FNB",
                    accountNumber: "00000000000",
                    branchCode: "250655")
            ],

            "Acme" =>
            [
                new BusinessProfileM(
                    businessName: "Acme Corporation",
                    email: "info@acme.co.za",
                    phone: "021 123 4567",
                    addressLine1: "12 Long Street",
                    city: "Cape Town",
                    province: "Western Cape",
                    postalCode: "8001",
                    country: "South Africa",
                    bankName: "FNB",
                    accountNumber: "62012345678",
                    branchCode: "250655")
            ],

            _ => // Default (fallback)
            [
                new BusinessProfileM(
                    businessName: $"{tenantName} Business",
                    email: $"info@{tenantName.ToLower()}.co.za",
                    phone: "081 000 0000",
                    addressLine1: "Default Address",
                    city: "Johannesburg",
                    province: "Gauteng",
                    postalCode: "2000",
                    country: "South Africa",
                    bankName: "Standard Bank",
                    accountNumber: "1234567890",
                    branchCode: "051001")
            ]
        };

        await context.BusinessProfiles.AddRangeAsync(businessProfiles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCustomersAsync(InventoryDbContext context, string tenantName)
    {
        if (await context.Customers.AnyAsync()) return;

        CustomerM[] customers = tenantName switch
        {
            "Demo" =>
            [
                new CustomerM(
                    name: "Demo Customer",
                    email: "demo@customer.com",
                    phone: "082 000 0000",
                    addressLine1: "Demo Address",
                    city: "Cape Town",
                    province: "Western Cape",
                    postalCode: "8001",
                    companyName: "Demo Company")
            ],

            "Acme" =>
            [
                new CustomerM(
                    name: "John Acme",
                    email: "john@acme.co.za",
                    phone: "082 111 1111",
                    addressLine1: "Acme Street",
                    city: "Cape Town",
                    province: "Western Cape",
                    postalCode: "8001",
                    companyName: "Acme Corp"),

                new CustomerM(
                    name: "Sarah Acme",
                    email: "sarah@acme.co.za",
                    phone: "083 222 2222",
                    addressLine1: "Acme Street",
                    city: "Cape Town",
                    province: "Western Cape",
                    postalCode: "8001",
                    companyName: "Acme Corp")
            ],

            _ => // Default
            [
                new CustomerM(
                    name: $"{tenantName} Customer 1",
                    email: $"customer1@{tenantName.ToLower()}.co.za",
                    phone: "082 123 4567",
                    addressLine1: "Default Address",
                    city: "Durban",
                    province: "KwaZulu-Natal",
                    postalCode: "4001",
                    companyName: $"{tenantName} Ltd"),

                new CustomerM(
                    name: $"{tenantName} Customer 2",
                    email: $"customer2@{tenantName.ToLower()}.co.za",
                    phone: "083 234 5678",
                    addressLine1: "Default Address",
                    city: "Johannesburg",
                    province: "Gauteng",
                    postalCode: "2001",
                    companyName: $"{tenantName} Ltd")
            ]
        };

        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
    }
}