using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Data;

public interface IInventoryDbContext
{
    DbSet<CategoryM> Categories { get; }
    DbSet<ItemM> Items { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}