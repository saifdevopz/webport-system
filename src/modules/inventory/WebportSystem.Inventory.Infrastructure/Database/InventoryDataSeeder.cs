using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;
using WebportSystem.Inventory.Domain.Entities.Category;
using WebportSystem.Inventory.Domain.Entities.Customer;
using WebportSystem.Inventory.Domain.Entities.Invoice;
using WebportSystem.Inventory.Domain.Entities.Item;

namespace WebportSystem.Inventory.Infrastructure.Database;

public static class InventoryDataSeeder
{
    public static async Task SeedAsync(InventoryDbContext context, string tenantName)
    {
        try
        {
            await SeedCategoriesAsync(context);
            await SeedItemsAsync(context);
            await SeedBusinessProfilesAsync(context, tenantName);
            await SeedCustomersAsync(context, tenantName);
            await SeedInvoicesAsync(context);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Seeding failed for {tenantName}: {ex}");
            throw;
        }
    }

    private static async Task SeedCategoriesAsync(InventoryDbContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        var categories = new List<CategoryM>
        {
            CategoryM.Create("ELEC", "Electronics"),
            CategoryM.Create("FASH", "Fashion"),
            CategoryM.Create("HOME", "Home & Living"),
            CategoryM.Create("FOOD", "Food & Beverages"),
            CategoryM.Create("HLTH", "Health & Wellness"),
            CategoryM.Create("SPRT", "Sports & Outdoors"),
            CategoryM.Create("AUTO", "Automotive"),
            CategoryM.Create("OFFC", "Office Supplies"),
            CategoryM.Create("TOYS", "Toys & Games"),
            CategoryM.Create("BOOK", "Books & Stationery")
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedItemsAsync(InventoryDbContext context)
    {
        if (await context.Items.AnyAsync()) return;

        var categories = await context.Categories.ToListAsync();

        // Helper to get category ID safely
        int GetCategoryId(string code) =>
            categories.First(_ => _.CategoryCode == code).CategoryId;

        var items = new List<ItemM>
        {
            // Electronics
            ItemM.Create(GetCategoryId("ELEC"), "TV001", "Samsung 55\" Smart TV", 8999, 6500),
            ItemM.Create(GetCategoryId("ELEC"), "PHN001", "iPhone 14", 15999, 12000),

            // Fashion
            ItemM.Create(GetCategoryId("FASH"), "SHRT01", "Men's Casual Shirt", 399, 200),
            ItemM.Create(GetCategoryId("FASH"), "JEAN01", "Slim Fit Jeans", 699, 400),

            // Home
            ItemM.Create(GetCategoryId("HOME"), "SOFA01", "3-Seater Sofa", 4999, 3000),
            ItemM.Create(GetCategoryId("HOME"), "LAMP01", "Bedside Lamp", 299, 120),

            // Food
            ItemM.Create(GetCategoryId("FOOD"), "COF001", "Ground Coffee 500g", 120, 70),
            ItemM.Create(GetCategoryId("FOOD"), "SNK001", "Potato Chips", 25, 10),

            // Health
            ItemM.Create(GetCategoryId("HLTH"), "VITC01", "Vitamin C Tablets", 150, 80),

            // Sports
            ItemM.Create(GetCategoryId("SPRT"), "BALL01", "Soccer Ball", 250, 120),

            // Automotive
            ItemM.Create(GetCategoryId("AUTO"), "OIL01", "Engine Oil 5W-30", 450, 300),

            // Office
            ItemM.Create(GetCategoryId("OFFC"), "PEN001", "Ballpoint Pens (Pack)", 50, 20),

            // Toys
            ItemM.Create(GetCategoryId("TOYS"), "CAR001", "Toy Car", 120, 60),

            // Books
            ItemM.Create(GetCategoryId("BOOK"), "NOTE01", "Notebook A4", 35, 15)
        };

        await context.Items.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBusinessProfilesAsync(InventoryDbContext context, string tenantName)
    {
        var exists = await context.BusinessProfiles.AnyAsync();

        if (exists) return;

#pragma warning disable S1075 // URIs should not be hardcoded
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
            branchCode: "051001",
            logoUrl: "https://webport-pull-zone.b-cdn.net/business-logos/logo-generic.png"
        );
#pragma warning restore S1075 // URIs should not be hardcoded

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

    private static async Task SeedInvoicesAsync(InventoryDbContext context)
    {
        if (await context.Invoices.AnyAsync())
            return;

        var items = await context.Items.ToListAsync();

        // Helper
        ItemM GetItem(string code) =>
            items.First(x => x.ItemCode == code);

        var invoices = new List<InvoiceM>();

        var today = DateOnly.FromDateTime(DateTime.Now);

        // ---------------------------
        // Invoice 1
        // ---------------------------
        var inv1 = InvoiceM.Create(today, today.AddDays(30), 1, "Walk-in Customer");

        var tv = GetItem("TV001");
        var chips = GetItem("SNK001");

        inv1.AddItem(tv.ItemId, tv.ItemDesc, tv.SellingPrice, 1);
        inv1.AddItem(chips.ItemId, chips.ItemDesc, chips.SellingPrice, 3);

        invoices.Add(inv1);

        // ---------------------------
        // Invoice 2
        // ---------------------------
        var inv2 = InvoiceM.Create(today, today.AddDays(30), 2, "Walk-in Customer");

        var phone = GetItem("PHN001");
        var coffee = GetItem("COF001");

        inv2.AddItem(phone.ItemId, phone.ItemDesc, phone.SellingPrice, 1);
        inv2.AddItem(coffee.ItemId, coffee.ItemDesc, coffee.SellingPrice, 2);

        invoices.Add(inv2);

        // ---------------------------
        // Invoice 3
        // ---------------------------
        var inv3 = InvoiceM.Create(today, today.AddDays(30), 2, "Walk-in Customer");

        var shirt = GetItem("SHRT01");
        var jeans = GetItem("JEAN01");

        inv3.AddItem(shirt.ItemId, shirt.ItemDesc, shirt.SellingPrice, 2);
        inv3.AddItem(jeans.ItemId, jeans.ItemDesc, jeans.SellingPrice, 1);

        invoices.Add(inv3);

        await context.Invoices.AddRangeAsync(invoices);
        await context.SaveChangesAsync();
    }
}