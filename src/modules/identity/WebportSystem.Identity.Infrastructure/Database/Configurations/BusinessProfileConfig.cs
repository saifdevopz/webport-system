using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Identity.Domain.BusinessProfile;

namespace WebportSystem.Inventory.Infrastructure.Database.Configurations;

public sealed class BusinessProfileConfig : IEntityTypeConfiguration<BusinessProfileM>
{
    public void Configure(EntityTypeBuilder<BusinessProfileM> builder)
    {
        builder.HasKey(_ => _.BusinessProfileId);

        builder.HasKey(_ => _.TenantId);

        builder.HasOne(_ => _.Tenant)
            .WithOne()
            .HasForeignKey<BusinessProfileM>(_ => _.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(_ => _.BusinessName)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(_ => _.Email)
            .IsRequired()
            .HasMaxLength(50);
    }
}