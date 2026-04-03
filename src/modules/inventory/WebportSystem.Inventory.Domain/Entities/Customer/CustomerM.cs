namespace WebportSystem.Inventory.Domain.Entities.Customer;

public sealed class CustomerM(
    string name,
    string email,
    string phone,
    string addressLine1,
    string city,
    string province,
    string postalCode,
    string companyName) : AggregateRoot, IMustHaveTenant
{
    public int TenantId { get; set; }
    public int CustomerId { get; set; }
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public string Phone { get; private set; } = phone;
    public string CompanyName { get; private set; } = companyName;
    public string AddressLine1 { get; private set; } = addressLine1;
    public string City { get; private set; } = city;
    public string Province { get; private set; } = province;
    public string PostalCode { get; private set; } = postalCode;
}