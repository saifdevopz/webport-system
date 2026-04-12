using System.ComponentModel.DataAnnotations;

namespace WebportSystem.Common.Contracts.Inventory;

public class ItemDto
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemDesc { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public decimal CostPrice { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; } = new();
}

public class ItemCommandDto
{
    public int CategoryId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemDesc { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public decimal CostPrice { get; set; }
}