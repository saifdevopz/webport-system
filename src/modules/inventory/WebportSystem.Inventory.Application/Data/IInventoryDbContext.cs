using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;
using WebportSystem.Inventory.Domain.Entities.Customer;
using WebportSystem.Inventory.Domain.Entities.Invoice;

namespace WebportSystem.Inventory.Application.Data;

public interface IInventoryDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    DbSet<BusinessProfileM> BusinessProfiles { get; }
    DbSet<CustomerM> Customers { get; }
    DbSet<CategoryM> Categories { get; }
    DbSet<ItemM> Items { get; }

    DbSet<InvoiceM> Invoices { get; }
    DbSet<InvoiceItemM> InvoiceItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}