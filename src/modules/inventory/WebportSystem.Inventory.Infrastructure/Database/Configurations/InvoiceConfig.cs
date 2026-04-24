using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Inventory.Domain.Entities.Invoice;

namespace WebportSystem.Inventory.Infrastructure.Database.Configurations;

public class InvoiceConfig : IEntityTypeConfiguration<InvoiceM>
{
    public void Configure(EntityTypeBuilder<InvoiceM> builder)
    {        
        builder.HasKey(x => x.InvoiceId);

        builder.Property(x => x.SubTotal)
            .HasPrecision(18, 2);

        builder.Property(x => x.Total)
            .HasPrecision(18, 2);

        // 🔗 Relationship (Aggregate)
        builder.HasMany(x => x.Items)
            .WithOne(_ => _.Invoice)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Customer FK (optional)
        builder.HasOne(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.CustomerId)
               .OnDelete(DeleteBehavior.SetNull);

        // 🚀 Backing field (DDD pattern)
        builder.Metadata
            .FindNavigation(nameof(InvoiceM.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

