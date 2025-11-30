using WebportSystem.Common.Domain.Abstractions;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Domain.Tenants;

public sealed class TenantM : AggregateRoot
{
    public int TenantId { get; set; }
    public required string TenantName { get; set; }
    public DateTime LicenceExpiryDate { get; set; }
    public string? DatabaseConnectionString { get; set; }
    public ICollection<UserM> Users { get; set; } = [];
    public static TenantM Create(string tenantName, string databaseConnectionString = null!)
    {
        TenantM model = new()
        {
            TenantName = tenantName,
            LicenceExpiryDate = DateTime.UtcNow.AddDays(30),
            DatabaseConnectionString = databaseConnectionString
        };

        return model;
    }
}