namespace WebportSystem.Common.Contracts.Inventory;

public class InvoiceItemDto
{
    public int ItemId { get; set; }

    public string ItemDesc { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal Total { get; set; }
}

public class InvoiceDto
{
    public int InvoiceId { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public string BusinessName { get; set; } = string.Empty;

    public int BusinessProfileId { get; set; }

    public int? CustomerId { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Total { get; set; }

    public List<InvoiceItemDto> Items { get; set; } = [];
}

public class InvoicePrintDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessAddress { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;

    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }

    public List<InvoiceItemDto> Items { get; set; } = new();
}

public sealed record CreateInvoiceItem(
    int ItemId,
    int Quantity
);
