using System.ComponentModel.DataAnnotations;

namespace WebportSystem.Common.Domain.Contracts.Inventory;

public class ItemDto
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemDesc { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; } = new();
}