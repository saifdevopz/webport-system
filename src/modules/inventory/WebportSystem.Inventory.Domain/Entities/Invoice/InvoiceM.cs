using WebportSystem.Inventory.Domain.Entities.BusinessProfile;
using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Domain.Entities.Invoice;

public sealed class InvoiceM : AggregateRoot
{
    public int InvoiceId { get; set; }
    public string InvoiceNumber { get; private set; } = string.Empty;
    public int BusinessProfileId { get; private set; }
    public int? CustomerId { get; private set; }
    public decimal SubTotal { get; private set; }
    public decimal Total { get; private set; }

    // 🔗 Navigation properties
    public BusinessProfileM BusinessProfile { get; private set; } = default!;
    public CustomerM Customer { get; private set; } = default!;

    private readonly List<InvoiceItemM> _items = [];
    public IReadOnlyCollection<InvoiceItemM> Items => _items;

    private InvoiceM() { } // EF

    public InvoiceM(
        string invoiceNumber,
        int businessProfileId,
        int? customerId)
    {
        InvoiceNumber = invoiceNumber;
        BusinessProfileId = businessProfileId;
        CustomerId = customerId;
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

        foreach (var item in items)
        {
            AddItem(item.itemId, item.name, item.price, item.qty);
        }

        Recalculate();
    }
}