namespace WebportSystem.Inventory.Domain.Entities.Customer;

public sealed class CustomerM(
    string name,
    string email,
    string phone,
    string companyName,
    string addressLine1,
    string postalCode,
    string city,
    string province) : AggregateRoot
{
    public int CustomerId { get; set; }
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public string Phone { get; private set; } = phone;
    public string CompanyName { get; private set; } = companyName;
    public string AddressLine1 { get; private set; } = addressLine1;
    public string PostalCode { get; private set; } = postalCode;
    public string City { get; private set; } = city;
    public string Province { get; private set; } = province;
}