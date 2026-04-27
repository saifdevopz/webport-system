using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WebportSystem.Common.Contracts.Inventory;

namespace WebportSystem.Common.Infrastructure.QuestPDF;

public class InvoiceItemDocument(InvoicePrintDto model) : IDocument
{
    public static Image LogoImage { get; } = Image.FromFile(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png"));

    public InvoicePrintDto Model { get; } = model;

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.ConstantItem(175).Element(x =>
            {
                var imageBytes = LoadImageFromUrl(Model.LogoUrl);
                if (imageBytes != null)
                {
                    x.Image(imageBytes);
                }
            });

            row.RelativeItem()
                .AlignRight()
                .Column(column =>
                {
                    column
                        .Item().Text("TAX INVOICE")
                        .FontSize(25).SemiBold().FontColor(Colors.Black);

                    column.Item().Text("");

                    column
                        .Item().AlignRight().Text(Model.BusinessName)
                        .FontSize(10).Bold().FontColor(Colors.Black);

                    column
                        .Item().AlignRight().Text(Model.BusinessAddress)
                        .FontSize(10).FontColor(Colors.Black);

                    column
                        .Item().AlignRight().Text(Model.BusinessPostalCode)
                        .FontSize(10).FontColor(Colors.Black);

                    column
                        .Item().AlignRight().Text(Model.BusinessCity)
                        .FontSize(10).FontColor(Colors.Black);

                    column
                        .Item().AlignRight().Text(Model.BusinessProvince)
                        .FontSize(10).FontColor(Colors.Black);

                    column
                        .Item().AlignRight().Text("South Africa")
                        .FontSize(10).FontColor(Colors.Black);

                    column.Item().Text("");

                    column
                        .Item().AlignRight().Text(Model.BusinessPhoneNumber)
                        .FontSize(10).FontColor(Colors.Black);
                });
        });
    }

    private static byte[]? LoadImageFromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        using var http = new HttpClient();
        return http.GetByteArrayAsync(url).Result;
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(10);

            column.Item().LineHorizontal(1);

            column.Item().Row(row =>
            {
                // Customer Details
                row.RelativeItem()
                   .AlignLeft()
                   .Column(col =>
                {
                    col.Item().Text("Bill To").SemiBold();
                    col.Item().Text("").SemiBold();
                    col.Spacing(2);

                    col.Item().Text(Model.CustomerName);
                    col.Item().Text(Model.CustomerBusinessName);
                });

                // Invoice Details
                row.RelativeItem()
                    .AlignRight()
                    .Column(col =>
                    {
                        col.Spacing(2);

                        void AddLine(string label, string value)
                        {
                            col.Item().AlignRight().Row(r =>
                            {
                                r.ConstantItem(140) // 👈 fixed width for ALL labels
                                    .AlignRight()
                                    .PaddingRight(10)
                                    .Text(label)
                                    .FontSize(10)
                                    .Bold();

                                r.RelativeItem()
                                    .AlignLeft()
                                    .Text(value)
                                    .FontSize(10)
                                    .FontColor(Colors.Black);
                            });
                        }

                        AddLine("Invoice Number:", Model.InvoiceId.ToString());
                        AddLine("Invoice Date:  ", Model.InvoiceDate.ToString("yyyy-MM-dd"));
                        AddLine("Payment Due Date:  ", Model.DueDate.ToString("yyyy-MM-dd") ?? "-");
                        AddLine("Amount Due (ZAR):  ", $"R {Model.Total}");
                    });
            });

            column.Item().Element(ComposeTable);

            var totalPrice = Model.Total.ToString();
            column.Item()
                .PaddingRight(5)
                .AlignRight()
                .Text($"Grand total: R {totalPrice}")
                .SemiBold();

            if (!string.IsNullOrWhiteSpace("COmments"))
                column.Item()
                    .PaddingTop(25)
                    .Element(ComposeComments);
        });
    }

    void ComposeTable(IContainer container)
    {
        var headerStyle = TextStyle.Default.SemiBold();

        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Text("#");
                header.Cell().Text("Items").Style(headerStyle);
                header.Cell().AlignRight().Text("Quantity").Style(headerStyle);
                header.Cell().AlignRight().Text("Unit Price").Style(headerStyle);
                header.Cell().AlignRight().Text("Total").Style(headerStyle);

                header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
            });

            foreach (var item in Model.Items!)
            {
                var index = Model.Items.ToList().IndexOf(item) + 1;

                table.Cell().Element(CellStyle).Text($"{index}");
                table.Cell().Element(CellStyle).Text(item.ItemDesc);
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}");
                table.Cell().Element(CellStyle).AlignRight().Text($"R{item.Total}");
                table.Cell().Element(CellStyle).AlignRight().Text($"R{item.Total * item.Quantity}");

                static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }
        });
    }
    void ComposeComments(IContainer container)
    {
        container.ShowEntire().Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Notes/Terms").FontSize(14).SemiBold();
            column.Item().Text(Model.Notes);
        });
    }
}

