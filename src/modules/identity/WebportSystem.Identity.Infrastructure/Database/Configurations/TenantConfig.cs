using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Identity.Domain.Tenants;

namespace WebportSystem.Identity.Infrastructure.Database.Configurations;

internal sealed class TenantConfig : IEntityTypeConfiguration<TenantM>
{
    public void Configure(EntityTypeBuilder<TenantM> builder)
    {
        // Table
        builder.ToTable("Tenants");

        builder
            .HasKey(_ => _.TenantId);

        builder.Property(_ => _.TenantName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(_ => _.LicenseExpiryDateUtc)
            .IsRequired();

        builder.HasIndex(_ => new { _.TenantName })
            .IsUnique();
    }
}