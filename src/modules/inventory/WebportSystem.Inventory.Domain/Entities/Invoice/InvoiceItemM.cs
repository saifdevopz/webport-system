namespace WebportSystem.Inventory.Domain.Entities.Invoice;

public class InvoiceItemM
{
    public int InvoiceItemId { get; set; }
    public int InvoiceId { get; set; }
    public InvoiceM Invoice { get; private set; } = default!;
    public int? ItemId { get; private set; }
    public string ItemDesc { get; private set; } = default!;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal Total { get; private set; }

    private InvoiceItemM() { }

    internal InvoiceItemM(
        int? itemId,
        string itemDesc,
        decimal unitPrice,
        int quantity)
    {
        ItemId = itemId;
        ItemDesc = itemDesc;
        UnitPrice = unitPrice;

        SetQuantity(quantity);

        Recalculate();
    }

    internal void SetQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        Quantity = quantity;
    }

    internal void Recalculate()
    {
        Total = (UnitPrice * Quantity);

        if (Total < 0)
            Total = 0;
    }
}