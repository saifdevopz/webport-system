namespace WebportSystem.Common.Domain.Contracts.Inventory;

public sealed class CustomerDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string CompanyName { get; set; } = default!;
    public string AddressLine1 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}