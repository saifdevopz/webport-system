using WebportSystem.Common.Contracts.Inventory;

namespace WebportSystem.Dashboard.Components.Pages.Tenant.Inventory.Invoice;

public class InvoiceViewModel
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime? InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string Currency { get; set; } = "ZAR";
    public decimal TaxPercent { get; set; }
    public decimal Discount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<InvoiceItemDto> Items { get; set; } = new();

    public static InvoiceViewModel CreateDefault()
    {
        return new InvoiceViewModel
        {
            InvoiceNumber = "INV-2026-001",
            InvoiceDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(14),
            TaxPercent = 10,
            Items =
            [
                new() {
                    ItemDesc = "Design services",
                    Quantity = 1,
                    UnitPrice = 1200m,
                }
            ]
        };
    }

    public static InvoiceViewModel CreateSample()
    {
        return new InvoiceViewModel
        {
            InvoiceNumber = "INV-2026-014",
            InvoiceDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(21),
            Currency = "ZAR",
            TaxPercent = 8.5m,
            Discount = 150m,
            Notes = "Payment due within 21 days. Please include invoice number with transfer reference.",
            Items =
            [
                new() {
                    ItemDesc = "Brand strategy workshop",
                    Quantity = 1,
                    UnitPrice = 1800m
                },
                new() {
                    ItemDesc = "UI design sprint",
                    Quantity = 3,
                    UnitPrice = 950m
                },
                new() {
                    ItemDesc = "Design system handoff",
                    Quantity = 1,
                    UnitPrice = 1200m
                }
            ]
        };
    }
}
