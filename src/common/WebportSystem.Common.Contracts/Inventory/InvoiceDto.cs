namespace WebportSystem.Common.Contracts.Inventory;

public class InvoiceItemDto
{
    public int? ItemId { get; set; }

    public string ItemDesc { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public decimal Quantity { get; set; } = 1;

    public decimal Total { get; set; }
}

public class InvoiceDto
{
    public int InvoiceId { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime? InvoiceDate { get; set; }

    public int? CustomerId { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Total { get; set; }

    public string? Notes { get; set; } = string.Empty;

    public List<InvoiceItemDto> Items { get; set; } = [];
}

public class InvoicePrintDto
{
    // Invoice Details
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }

    // Business Details
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessAddress { get; set; } = string.Empty;
    public string BusinessPostalCode { get; set; } = string.Empty;
    public string BusinessCity { get; set; } = string.Empty;
    public string BusinessProvince { get; set; } = string.Empty;

    // Customer Details
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerBusinessName { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;

    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }

    public List<InvoiceItemDto> Items { get; set; } = new();
}

public sealed record CreateInvoiceItem(
    int ItemId,
    int Quantity
);
