namespace WebportSystem.Common.Domain.Contracts.Identity;

public class GetTenantDto
{
    public int TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public DateTime LicenceExpiryDate { get; set; }
}


public record TenantsWrapper<T>(IEnumerable<T> Tenants);
public record TenantWrapper<T>(T Tenant);