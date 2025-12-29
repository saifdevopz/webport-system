namespace WebportSystem.Inventory.Domain.Entities.Category;

public sealed class CategoryM : AggregateRoot, IMustHaveTenant
{
    public int TenantId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryCode { get; private set; } = string.Empty;
    public string CategoryDesc { get; set; } = string.Empty;

    private CategoryM() { }

    public static CategoryM Create
    (
        string categoryCode,
        string categoryDesc
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(categoryCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(categoryDesc);

        CategoryM model = new()
        {
            CategoryCode = categoryCode,
            CategoryDesc = categoryDesc,
        };

        return model;
    }
}
