using Microsoft.EntityFrameworkCore;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Application.Data;

public interface IUsersDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}