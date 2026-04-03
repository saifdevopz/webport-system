using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Infrastructure.Database.Configurations;

public sealed class CustomerConfig : IEntityTypeConfiguration<CustomerM>
{
    public void Configure(EntityTypeBuilder<CustomerM> builder)
    {
        builder.HasKey(_ => _.CustomerId);

        builder.HasIndex(_ => new { _.Name })
            .IsUnique();
    }
}