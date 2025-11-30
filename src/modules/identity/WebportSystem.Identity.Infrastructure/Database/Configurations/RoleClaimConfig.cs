using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Identity.Domain.Roles;

namespace WebportSystem.Identity.Infrastructure.Database.Configurations;

public class RoleClaimConfig : IEntityTypeConfiguration<RoleClaimM>
{
    public void Configure(EntityTypeBuilder<RoleClaimM> builder)
    {
        builder.ToTable("role_claims");
    }
}
