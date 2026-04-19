namespace WebportSystem.Common.Contracts.Identity;

public class TenantDto
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public DateTime LicenceExpiryDate { get; set; }
}