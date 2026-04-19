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
        string databaseName)
    {
        TenantM tenant = new()
        {
            TenantId = Guid.NewGuid(),
            TenantName = tenantName,
            DatabaseName = databaseName,
            DatabaseConnectionString = GetPostreSQLDatabaseConnectionString(databaseName),
            LicenseExpiryDateUtc = DateTime.UtcNow,
            Status = TenantStatus.Trial
        };

        return tenant;
    }

    private static string GetPostreSQLDatabaseConnectionString(string databaseName)
    {
        //return $"Host=102.211.206.231;Port=5432;Database={databaseName};Username=sword;Password=25122000@Saif;Pooling=true;MinPoolSize=10;MaxPoolSize=100;Include Error Detail=true;GSS Encryption Mode=Disable;";
        return $"Host=102.214.11.80; Database={databaseName}; Username=sword; Password=25122000@Saif; GSS Encryption Mode=Disable;";
    }
}



public enum TenantStatus
{
    Trial = 0,
    Active = 1,
    Suspended = 2,
    Expired = 3
}