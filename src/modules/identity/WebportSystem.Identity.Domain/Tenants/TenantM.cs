using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Domain.Tenants;

public sealed class TenantM : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public string TenantName { get; private set; } = string.Empty;
    public string DatabaseName { get; private set; } = string.Empty;
    public string DatabaseConnectionString { get; private set; } = string.Empty;
    public DateTime LicenseExpiryDateUtc { get; private set; }
    public TenantStatus Status { get; private set; }
    public ICollection<UserM> Users { get; set; } = [];

    public static TenantM Create(
        string tenantName,
        string databaseName,
        string databaseConnectionString)
    {
        TenantM tenant = new()
        {
            TenantId = Guid.NewGuid(),
            TenantName = tenantName,
            DatabaseName = databaseName,
            DatabaseConnectionString = databaseConnectionString,
            LicenseExpiryDateUtc = DateTime.UtcNow,
            Status = TenantStatus.Trial
        };

        return tenant;
    }
}

public enum TenantStatus
{
    Trial = 0,
    Active = 1,
    Suspended = 2,
    Expired = 3
}