namespace WebportSystem.Common.Domain.Contracts.Inventory;

public sealed record CustomerDto(
    int CustomerId,
    string Name,
    string Email,
    string Phone,
    string CompanyName,
    string AddressLine1,
    string City,
    string Province,
    string PostalCode);