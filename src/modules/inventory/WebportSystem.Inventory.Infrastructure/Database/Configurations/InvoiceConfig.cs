using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Inventory.Domain.Entities.Invoice;

namespace WebportSystem.Inventory.Infrastructure.Database.Configurations;

public class InvoiceConfig : IEntityTypeConfiguration<InvoiceM>
{
    public void Configure(EntityTypeBuilder<InvoiceM> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(x => x.InvoiceId);

        builder.Property(x => x.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.SubTotal)
            .HasPrecision(18, 2);

        builder.Property(x => x.Total)
            .HasPrecision(18, 2);

        // 🔗 Relationship (Aggregate)
        builder.HasMany(x => x.Items)
            .WithOne(_ => _.Invoice)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // BusinessProfile FK
        builder.HasOne(x => x.BusinessProfile)
               .WithMany()
               .HasForeignKey(x => x.BusinessProfileId)
               .OnDelete(DeleteBehavior.Restrict);

        // Customer FK (optional)
        builder.HasOne(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.CustomerId)
               .OnDelete(DeleteBehavior.SetNull);

        // 🚀 Backing field (DDD pattern)
        builder.Metadata
            .FindNavigation(nameof(InvoiceM.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => new { x.InvoiceNumber })
            .IsUnique();
    }
}

