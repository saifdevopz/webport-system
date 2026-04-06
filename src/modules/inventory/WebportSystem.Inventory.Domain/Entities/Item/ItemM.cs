using WebportSystem.Inventory.Domain.Entities.Category;

namespace WebportSystem.Inventory.Domain.Entities.Item;

public sealed class ItemM : AggregateRoot
{
    public int ItemId { get; set; }
    public int CategoryId { get; set; }
    public CategoryM? Category { get; set; }
    public required string ItemCode { get; set; }
    public required string ItemDesc { get; set; }

    public static ItemM Create
    (
        int categoryId,
        string itemCode,
        string itemDesc
    )
    {
        ItemM model = new()
        {
            CategoryId = categoryId,
            ItemCode = itemCode,
            ItemDesc = itemDesc,
        };

        return model;
    }
}