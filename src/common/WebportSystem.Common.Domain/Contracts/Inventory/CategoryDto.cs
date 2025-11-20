using System.ComponentModel.DataAnnotations;

namespace WebportSystem.Common.Domain.Contracts.Inventory;

public class CategoryDto
{
    public int CategoryId { get; set; }

    [Required]
    public string CategoryCode { get; set; } = string.Empty;

    [Required]
    public string CategoryDesc { get; set; } = string.Empty;
}