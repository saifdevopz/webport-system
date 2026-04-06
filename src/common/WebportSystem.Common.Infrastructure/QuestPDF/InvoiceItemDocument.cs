using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WebportSystem.Common.Domain.Contracts.Inventory;

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

    public void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.ConstantItem(175).Image(LogoImage);

            row.RelativeItem()
               .AlignRight()
               .Column(column =>
               {
                   column.Item().Text("TAX INVOICE")
                         .FontSize(25).SemiBold().FontColor(Colors.Black);

                   column.Item().Text(""); // Spacer

                   column.Item().AlignRight().Text(Model.BusinessName ?? "Business Name")
                         .FontSize(10).Bold().FontColor(Colors.Black);

               });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(10);
            column.Item().LineHorizontal(1);



            column.Item().Element(ComposeTable);



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
                header.Cell().AlignRight().Text("Unit price").Style(headerStyle);
                header.Cell().AlignRight().Text("Quantity").Style(headerStyle);
                header.Cell().AlignRight().Text("Total").Style(headerStyle);

                header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
            });

            foreach (var item in Model.Items!)
            {
                var index = Model.Items.ToList().IndexOf(item) + 1;

                table.Cell().Element(CellStyle).Text($"{index}");

                static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }
        });
    }

}

