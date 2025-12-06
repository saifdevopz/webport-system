using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Inventory.Domain.Entities.Category;

namespace WebportSystem.Inventory.Infrastructure.Database.Configurations;

public sealed class CategoryConfig : IEntityTypeConfiguration<CategoryM>
{
    public void Configure(EntityTypeBuilder<CategoryM> builder)
    {
        builder.HasKey(_ => _.CategoryId);

        builder.HasIndex(_ => new { _.TenantId, _.CategoryCode })
            .IsUnique();

        builder.Property(_ => _.CategoryCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(_ => _.CategoryDesc)
            .IsRequired()
            .HasMaxLength(50);
    }
}