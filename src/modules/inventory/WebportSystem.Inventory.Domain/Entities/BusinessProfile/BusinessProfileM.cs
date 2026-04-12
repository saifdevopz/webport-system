namespace WebportSystem.Inventory.Domain.Entities.BusinessProfile;

public sealed class BusinessProfileM(
    string businessName,
    string email,
    string phone,
    string addressLine1,
    string city,
    string province,
    string postalCode,
    string country,
    string? bankName = null,
    string? accountNumber = null,
    string? branchCode = null,
    string? logoUrl = null) : AggregateRoot
{
    public int BusinessProfileId { get; set; }
    public string BusinessName { get; private set; } = businessName;
    public string Phone { get; private set; } = phone;
    public string Email { get; private set; } = email;

    // Address
    public string AddressLine1 { get; private set; } = addressLine1;
    public string PostalCode { get; private set; } = postalCode;
    public string City { get; private set; } = city;
    public string Province { get; private set; } = province;
    public string Country { get; private set; } = country;

    // Banking
    public string? BankName { get; private set; } = bankName;
    public string? BranchCode { get; private set; } = branchCode;
    public string? AccountNumber { get; private set; } = accountNumber;
    public string? LogoUrl { get; private set; } = logoUrl;

    public void Update(
      string businessName,
      string email,
      string phone,
      string addressLine1,
      string city,
      string province,
      string postalCode,
      string country,
      string? bankName = null,
      string? accountNumber = null,
      string? branchCode = null)
    {
        BusinessName = businessName;
        Email = email;
        Phone = phone;
        AddressLine1 = addressLine1;
        City = city;
        Province = province;
        PostalCode = postalCode;
        Country = country;
        BankName = bankName;
        AccountNumber = accountNumber;
        BranchCode = branchCode;
    }
}