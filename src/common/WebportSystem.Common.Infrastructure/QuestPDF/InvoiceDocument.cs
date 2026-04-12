using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace WebportSystem.Common.Infrastructure.QuestPDF;

public class InvoiceDocument(InvoiceModel model) : IDocument
{
    public static Image LogoImage { get; } = Image.FromFile(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png"));

    public InvoiceModel Model { get; } = model;

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

    public static void ComposeHeader(IContainer container)
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
                    .Item().AlignRight().Text($"MH MANUFACTURERS")
                    .FontSize(10).Bold().FontColor(Colors.Black);

                column
                    .Item().AlignRight().Text($"2 Court Ln")
                    .FontSize(10).FontColor(Colors.Black);

                column
                    .Item().AlignRight().Text($"Verula, KwaZulu-Natal 4340")
                    .FontSize(10).FontColor(Colors.Black);

                column
                    .Item().AlignRight().Text($"SOUTH AFRICA")
                    .FontSize(10).FontColor(Colors.Black);

                column.Item().Text("");

                column
                    .Item().AlignRight().Text($"0668732375")
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
            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressComponent("", Model.SellerAddress!));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressComponent("", Model.CustomerAddress!));
            });

            column.Item().Element(ComposeTable);

            var totalPrice = Model.Items!.Sum(x => x.Price * x.Quantity);
            column.Item().PaddingRight(5).AlignRight().Text($"Grand total: R {totalPrice}").SemiBold();

            if (!string.IsNullOrWhiteSpace(Model.Comments))
                column.Item().PaddingTop(25).Element(ComposeComments);
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
                table.Cell().Element(CellStyle).Text(item.Name);
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price:C}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity:C}");

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
            column.Item().Text(Model.Comments);
        });
    }
}

public class AddressComponent(string title, Address address) : IComponent
{
    private string Title { get; } = title;
    private Address Address { get; } = address;

    public void Compose(IContainer container)
    {
        container.ShowEntire().Column(column =>
        {
            column.Spacing(2);

            column.Item().Text(Title).SemiBold();


            column.Item().Text(Address.CompanyName);
            column.Item().Text(Address.Street);
            column.Item().Text($"{Address.City}, {Address.State}");
            column.Item().Text(Address.Email);
            column.Item().Text(Address.Phone);
        });
    }
}