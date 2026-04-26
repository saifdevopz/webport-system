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
    public int CustomerId { get; set; }
    public DateTime? InvoiceDate { get; set; } = DateTime.Today;
    public DateTime? DueDate { get; set; } = DateTime.Today;
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<InvoiceItemDto> Items { get; set; } = [];
}

public class InvoicePrintDto
{
    // Invoice Details    
    public int InvoiceId { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public DateOnly DueDate { get; set; }

    // Business Details
    public string LogoUrl { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessAddress { get; set; } = string.Empty;
    public string BusinessPostalCode { get; set; } = string.Empty;
    public string BusinessCity { get; set; } = string.Empty;
    public string BusinessProvince { get; set; } = string.Empty;
    public string BusinessPhoneNumber { get; set; } = string.Empty;

    // Customer Details
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerBusinessName { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;

    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<InvoiceItemDto> Items { get; set; } = new();
}

public sealed record CreateInvoiceItem(
    int ItemId,
    int Quantity
);
