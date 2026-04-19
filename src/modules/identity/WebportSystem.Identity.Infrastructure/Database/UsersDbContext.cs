using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Domain.Platform;
using WebportSystem.Identity.Domain.Roles;
using WebportSystem.Identity.Domain.Tenants;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Infrastructure.Database;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options)
    : IdentityDbContext<UserM,
                        RoleM,
                        string,
                        IdentityUserClaim<string>,
                        UserRoleM,
                        IdentityUserLogin<string>,
                        RoleClaimM,
                        IdentityUserToken<string>>(options), IUsersDbContext
{
    public DbSet<PlatformUserM> PlatformUsers => Set<PlatformUserM>();
    public DbSet<TenantM> Tenants => Set<TenantM>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Schema       
        builder.HasDefaultSchema("identity");

        // Configurations
        builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        optionsBuilder.UseExceptionProcessor();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}