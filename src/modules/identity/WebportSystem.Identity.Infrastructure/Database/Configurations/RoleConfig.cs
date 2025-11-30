using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Identity.Domain.Roles;

namespace WebportSystem.Identity.Infrastructure.Database.Configurations;

public class RoleConfig : IEntityTypeConfiguration<RoleM>
{
    public void Configure(EntityTypeBuilder<RoleM> builder)
    {
        builder.ToTable("roles");

        // Each Role can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        // Each Role can have many associated RoleClaims
        builder.HasMany(e => e.RoleClaims)
            .WithOne(e => e.Role)
            .HasForeignKey(rc => rc.RoleId)
            .IsRequired();
    }
}
