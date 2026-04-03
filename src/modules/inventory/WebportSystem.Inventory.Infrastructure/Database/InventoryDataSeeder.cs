using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;
using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Infrastructure.Database;

public static class InventoryDataSeeder
{
    public static async Task SeedAsync(InventoryDbContext context)
    {
        await SeedBusinessProfilesAsync(context);
        await SeedCustomersAsync(context);
    }

    private static async Task SeedBusinessProfilesAsync(InventoryDbContext context)
    {
        if (await context.BusinessProfiles.AnyAsync()) return;

        BusinessProfileM[] businessProfiles =
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
            {
                TenantId = 2
            },

            new BusinessProfileM(
                businessName: "Webport Technologies",
                email: "hello@webport.co.za",
                phone: "011 987 6543",
                addressLine1: "45 Sandton Drive",
                city: "Johannesburg",
                province: "Gauteng",
                postalCode: "2196",
                country: "South Africa",
                bankName: "Standard Bank",
                accountNumber: "00112233445",
                branchCode: "051001")
            {
                TenantId = 3
            },

            new BusinessProfileM(
                businessName: "Blue Ocean Traders",
                email: "contact@blueocean.co.za",
                phone: "031 456 7890",
                addressLine1: "8 Marine Parade",
                city: "Durban",
                province: "KwaZulu-Natal",
                postalCode: "4001",
                country: "South Africa",
                bankName: "Absa",
                accountNumber: "4072345678",
                branchCode: "632005")
            {
                TenantId = 4
            },
        ];

        await context.BusinessProfiles.AddRangeAsync(businessProfiles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCustomersAsync(InventoryDbContext context)
    {
        if (await context.Customers.AnyAsync()) return;

        CustomerM[] customers =
        [
            new CustomerM(
                name: "John Hendricks",
                email: "john.hendricks@gmail.com",
                phone: "082 123 4567",
                addressLine1: "15 Kloof Street",
                city: "Cape Town",
                province: "Western Cape",
                postalCode: "8001",
                companyName: "Hendricks Consulting")
            {
                TenantId = 2
            },

            new CustomerM(
                name: "Sarah Botha",
                email: "sarah.botha@outlook.com",
                phone: "083 234 5678",
                addressLine1: "32 Commissioner Street",
                city: "Johannesburg",
                province: "Gauteng",
                postalCode: "2001",
                companyName: "Botha & Associates")
            {
                TenantId = 2
            },

            new CustomerM(
                name: "Thabo Nkosi",
                email: "thabo.nkosi@nkosi.co.za",
                phone: "084 345 6789",
                addressLine1: "10 West Street",
                city: "Durban",
                province: "KwaZulu-Natal",
                postalCode: "4001",
                companyName: "Nkosi Trading")
            {
                TenantId = 3
            },

            new CustomerM(
                name: "Anele Dlamini",
                email: "anele.dlamini@dlamini.co.za",
                phone: "071 456 7890",
                addressLine1: "5 Church Square",
                city: "Pretoria",
                province: "Gauteng",
                postalCode: "0002",
                companyName: "Dlamini Enterprises")
            {
                TenantId = 4
            },

            new CustomerM(
                name: "Megan van der Merwe",
                email: "megan.vdm@vandermerwe.co.za",
                phone: "072 567 8901",
                addressLine1: "88 Adderley Street",
                city: "Cape Town",
                province: "Western Cape",
                postalCode: "8000",
                companyName: "Van der Merwe Group")
            {
                TenantId = 4
            },
        ];

        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
    }
}