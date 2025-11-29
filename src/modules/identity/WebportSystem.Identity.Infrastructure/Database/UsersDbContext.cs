using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Domain.Tenants;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Infrastructure.Database;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options)
    : IdentityDbContext<User, IdentityRole, string>(options), IUsersDbContext
{
    public DbSet<Tenant> Tenants => Set<Tenant>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Schema       
        builder.HasDefaultSchema("identity");

        // Configurations
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}