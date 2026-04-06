using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Application.Data;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;
using WebportSystem.Inventory.Domain.Entities.Category;
using WebportSystem.Inventory.Domain.Entities.Customer;
using WebportSystem.Inventory.Domain.Entities.Invoice;
using WebportSystem.Inventory.Domain.Entities.Item;
using WebportSystem.Inventory.Infrastructure.Common;

namespace WebportSystem.Inventory.Infrastructure.Database;

public sealed class InventoryDbContext(
    DbContextOptions<InventoryDbContext> options) : DbContext(options), IInventoryDbContext
{
    public DbSet<BusinessProfileM> BusinessProfiles => Set<BusinessProfileM>();
    public DbSet<CustomerM> Customers => Set<CustomerM>();
    public DbSet<CategoryM> Categories => Set<CategoryM>();
    public DbSet<ItemM> Items => Set<ItemM>();
    public DbSet<InvoiceM> Invoices => Set<InvoiceM>();
    public DbSet<InvoiceItemM> InvoiceItems => Set<InvoiceItemM>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Schema
        modelBuilder.HasDefaultSchema(InventoryConstants.Schema);

        // Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        // Interceptors
        optionsBuilder.AddInterceptors(new AuditableEntityInterceptor());
        optionsBuilder.AddInterceptors(new InsertOutboxMessagesInterceptor());
    }
}