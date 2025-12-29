using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebportSystem.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder), "OutboxMessageConfiguration - EntityTypeBuilder cannot be null");
        }

        builder.ToTable("outbox_messages");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Content).HasMaxLength(5000).HasColumnType("jsonb");
    }
}
