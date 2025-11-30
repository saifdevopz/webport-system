using Microsoft.EntityFrameworkCore;
using WebportSystem.Identity.Domain.Tenants;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Application.Data;

public interface IUsersDbContext
{
    DbSet<TenantM> Tenants { get; }
    DbSet<UserM> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}