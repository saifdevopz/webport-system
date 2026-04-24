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
            row.ConstantItem(175).Image(LogoImage);

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
                        .Item().AlignRight().Text(Model.CustomerAddress + "dd")
                        .FontSize(10).FontColor(Colors.Black);

                    column.Item().Text("");

                    column
                        .Item().AlignRight().Text("0668732375")
                        .FontSize(10).FontColor(Colors.Black);
                });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(10);

            column.Item().LineHorizontal(1);

            // Bill To section with text underneath
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Bill To").SemiBold();
                    col.Item().Text("").SemiBold();
                    col.Spacing(2);
                    // 👇 Add your extra text here
                    col.Item().Text(Model.CustomerName);
                    col.Item().Text(Model.CustomerBusinessName);
                    col.Item().Text("Address Line 1");
                    col.Item().Text("City, Postal Code");
                    col.Item().Text("Phone: 0123456789");
                });

                row.ConstantItem(50);

                row.RelativeItem().Text("ssss");
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
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Total:C}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Total * item.Quantity:C}");

                static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }
        });
    }
    void ComposeComments(IContainer container)
    {
        container.ShowEntire().Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Comments").FontSize(14).SemiBold();
            column.Item().Text(Model.BusinessName);
        });
    }
}

