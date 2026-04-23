using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Domain.Entities.Invoice;

public sealed class InvoiceM : AggregateRoot
{
    public int InvoiceId { get; set; }

    // Customer
    public int? CustomerId { get; set; }
    public CustomerM Customer { get; private set; } = default!;

    // Totals
    public decimal SubTotal { get; private set; }
    public decimal Total { get; private set; }

    private readonly List<InvoiceItemM> _items = [];
    public IReadOnlyCollection<InvoiceItemM> Items => _items;

    public static InvoiceM Create(int? customerId, string customerName)
    {
        InvoiceM model = new()
        {
            CustomerId = customerId,
        };

        return model;
    }

    public void RemoveItem(int itemId)
    {
        var item = _items.FirstOrDefault(x => x.ItemId == itemId);
        if (item == null) return;

        _items.Remove(item);
        Recalculate();
    }
    public void AddItem(
        int itemId,
        string itemName,
        decimal unitPrice,
        int quantity)
    {
        var item = new InvoiceItemM(
            itemId,
            itemName,
            unitPrice,
            quantity);

        _items.Add(item);

        Recalculate();
    }

    private void Recalculate()
    {
        SubTotal = _items.Sum(x => x.Total);
        Total = SubTotal < 0 ? 0 : SubTotal;
    }

    public void ReplaceItems(List<(int itemId, string name, decimal price, int qty)> items)
    {
        _items.Clear();

        foreach (var (itemId, name, price, qty) in items)
        {
            AddItem(itemId, name, price, qty);
        }

        Recalculate();
    }
}