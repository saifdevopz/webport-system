namespace WebportSystem.Dashboard.Components.Pages.UI;

public class InvoiceViewModel
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime? InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string Currency { get; set; } = "USD";
    public string FromCompany { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientAddress { get; set; } = string.Empty;
    public decimal TaxPercent { get; set; }
    public decimal Discount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<InvoiceLineItemModel> Items { get; set; } = new();

    public static InvoiceViewModel CreateDefault()
    {
        return new InvoiceViewModel
        {
            InvoiceNumber = "INV-2026-001",
            InvoiceDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(14),
            TaxPercent = 10,
            Items = new List<InvoiceLineItemModel>
            {
                new InvoiceLineItemModel
                {
                    Description = "Design services",
                    Quantity = 1,
                    Rate = 1200m
                }
            }
        };
    }

    public static InvoiceViewModel CreateSample()
    {
        return new InvoiceViewModel
        {
            InvoiceNumber = "INV-2026-014",
            InvoiceDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(21),
            Currency = "USD",
            FromCompany = "Northwind Creative Studio",
            FromEmail = "billing@northwindstudio.com",
            FromAddress = "451 Market Street\nSan Francisco, CA 94105",
            ClientName = "Apex Ventures",
            ClientEmail = "accounts@apexventures.com",
            ClientAddress = "780 Madison Avenue\nNew York, NY 10065",
            TaxPercent = 8.5m,
            Discount = 150m,
            Notes = "Payment due within 21 days. Please include invoice number with transfer reference.",
            Items = new List<InvoiceLineItemModel>
            {
                new InvoiceLineItemModel
                {
                    Description = "Brand strategy workshop",
                    Quantity = 1,
                    Rate = 1800m
                },
                new InvoiceLineItemModel
                {
                    Description = "UI design sprint",
                    Quantity = 3,
                    Rate = 950m
                },
                new InvoiceLineItemModel
                {
                    Description = "Design system handoff",
                    Quantity = 1,
                    Rate = 1200m
                }
            }
        };
    }
}

public class InvoiceLineItemModel
{
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; } = 1;
    public decimal Rate { get; set; }
}
