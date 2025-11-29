using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Identity.Domain.Tenants;

namespace WebportSystem.Identity.Infrastructure.Database.Configurations;
internal sealed class TenantConfig : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder
            .HasKey(_ => _.TenantId);

        builder.Property(_ => _.TenantName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(_ => _.LicenceExpiryDate)
            .IsRequired();

        builder.HasIndex(_ => new { _.TenantName })
            .IsUnique();
    }
}