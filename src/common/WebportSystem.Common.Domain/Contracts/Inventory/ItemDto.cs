using System.ComponentModel.DataAnnotations;

namespace WebportSystem.Common.Domain.Contracts.Inventory;

public class ItemDto
{
    public int ItemId { get; set; }

    [Required]
    public string ItemCode { get; set; } = string.Empty;

    [Required]
    public string ItemDesc { get; set; } = string.Empty;

    public CategoryDto Category { get; set; } = new();

    [Required]
    public int CategoryId { get; set; } = new();
}