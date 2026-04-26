using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Domain.Entities.Invoice;

public sealed class InvoiceM : AggregateRoot
{
    public int InvoiceId { get; set; }
    public DateOnly InvoiceDate { get; private set; }
    public DateOnly DueDate { get; private set; }

    // Customer
    public int CustomerId { get; set; }
    public CustomerM Customer { get; private set; } = default!;

    // Totals
    public decimal SubTotal { get; private set; }
    public decimal Total { get; private set; }
    public string Notes { get; private set; } = string.Empty;

    private readonly List<InvoiceItemM> _items = [];
    public IReadOnlyCollection<InvoiceItemM> Items => _items;

    public static InvoiceM Create(
        DateOnly invoiceDate,
        DateOnly dueDate,
        int customerId,
        string notes)
    {
        InvoiceM model = new()
        {
            InvoiceDate = invoiceDate,
            DueDate = dueDate,
            CustomerId = customerId,
            Notes = notes
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