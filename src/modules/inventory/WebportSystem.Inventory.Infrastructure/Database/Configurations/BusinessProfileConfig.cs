using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;

namespace WebportSystem.Inventory.Infrastructure.Database.Configurations;

public sealed class BusinessProfileConfig : IEntityTypeConfiguration<BusinessProfileM>
{
    public void Configure(EntityTypeBuilder<BusinessProfileM> builder)
    {        
        builder.HasKey(_ => _.BusinessProfileId);

        builder.Property(_ => _.BusinessName)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(_ => _.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(_ => _.BusinessProfileId)
            .ValueGeneratedOnAdd();
    }
}