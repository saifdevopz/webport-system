namespace WebportSystem.Common.Domain.Contracts.Inventory;

public sealed record BusinessProfileDto(
    string BusinessName,
    string Email,
    string Phone,
    string AddressLine1,
    string City,
    string Province,
    string PostalCode,
    string Country,
    string? BankName = null,
    string? AccountNumber = null,
    string? BranchCode = null);