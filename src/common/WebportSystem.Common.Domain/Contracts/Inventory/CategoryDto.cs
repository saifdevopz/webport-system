namespace WebportSystem.Common.Domain.Contracts.Inventory;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryDesc { get; set; } = string.Empty;
}