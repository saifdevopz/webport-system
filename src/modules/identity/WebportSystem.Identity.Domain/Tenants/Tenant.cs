using WebportSystem.Common.Domain.Abstractions;

namespace WebportSystem.Identity.Domain.Tenants;

public sealed class Tenant : AggregateRoot
{
    public int TenantId { get; set; }
    public required string TenantName { get; set; }
    public DateTime LicenceExpiryDate { get; set; }

    public static Tenant Create(string tenantName)
    {
        Tenant model = new()
        {
            TenantName = tenantName,
            LicenceExpiryDate = DateTime.UtcNow.AddDays(30),
        };

        return model;
    }
}