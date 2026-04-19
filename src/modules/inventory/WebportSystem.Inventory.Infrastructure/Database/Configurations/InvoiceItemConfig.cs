using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Inventory.Domain.Entities.Invoice;

namespace WebportSystem.Inventory.Infrastructure.Database.Configurations;

public class InvoiceItemConfig : IEntityTypeConfiguration<InvoiceItemM>
{
    public void Configure(EntityTypeBuilder<InvoiceItemM> builder)
    {
        builder.ToTable("InvoiceItems");

        // 🔑 Key
        builder.HasKey(x => x.InvoiceItemId);

        // 🔗 FK
        builder.Property(x => x.InvoiceId)
            .IsRequired();

        builder.HasIndex(x => x.InvoiceId);

        // 🧾 Snapshot
        builder.Property(x => x.ItemDesc)
            .IsRequired()
            .HasMaxLength(200);

        // 💰 Money
        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.Total)
            .HasPrecision(18, 2);

        // 📦 Quantity
        builder.Property(x => x.Quantity)
            .IsRequired();

        // 🔗 Item reference (no FK constraint needed if loosely coupled)
        builder.Property(x => x.ItemId)
            .IsRequired();
    }
}