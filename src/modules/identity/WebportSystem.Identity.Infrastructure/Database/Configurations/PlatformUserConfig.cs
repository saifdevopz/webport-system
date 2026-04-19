using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Identity.Domain.Platform;

namespace WebportSystem.Identity.Infrastructure.Database.Configurations;

public class PlatformUserConfig : IEntityTypeConfiguration<PlatformUserM>
{
    public void Configure(EntityTypeBuilder<PlatformUserM> builder)
    {
        // Table
        builder.ToTable("PlatformUsers");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Email
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256)
            .IsUnicode(false);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        // Password Hash
        builder.Property(x => x.Password)
            .IsRequired();

        // Display Name
        builder.Property(x => x.DisplayName)
            .HasMaxLength(200);
    }
}