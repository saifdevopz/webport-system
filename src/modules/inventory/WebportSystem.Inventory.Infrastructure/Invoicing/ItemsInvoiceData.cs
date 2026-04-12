using QuestPDF.Helpers;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Infrastructure.QuestPDF;
using WebportSystem.Inventory.Application.Features.Item;

namespace WebportSystem.Inventory.Infrastructure.Invoicing;

public class ItemsInvoiceData(
    IQueryHandler<GetItemsQuery, List<ItemDto>> handler)
{
    private static readonly Random Random = new();

    public async Task<InvoiceModel> GetInvoiceDetails(
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(new GetItemsQuery(), cancellationToken);

        var items = result.IsSuccess
            ? result.Data
                .Select(i => new OrderItem
                {
                    Name = i.ItemDesc,
                    Price = 50,
                    Quantity = Random.Next(1, 10)
                })
                .ToList()
            : [.. Enumerable
                .Range(1, 25)
                .Select(_ => GenerateRandomOrderItem())];

        return new InvoiceModel
        {
            InvoiceNumber = Random.Next(1_000, 10_000),
            IssueDate = DateTime.Now,
            DueDate = DateTime.Now + TimeSpan.FromDays(14),
            SellerAddress = GenerateRandomAddress(),
            CustomerAddress = GenerateRandomAddress(),
            Items = items,
            Comments = Placeholders.Paragraph()
        };
    }

    private static OrderItem GenerateRandomOrderItem()
    {
        return new OrderItem
        {
            Name = Placeholders.Label(),
            Price = (decimal)Math.Round(Random.NextDouble() * 100, 2),
            Quantity = Random.Next(1, 10)
        };
    }

    private static Address GenerateRandomAddress()
    {
        return new Address
        {
            CompanyName = Placeholders.Name(),
            Street = Placeholders.Label(),
            City = Placeholders.Label(),
            State = Placeholders.Label(),
            Email = Placeholders.Email(),
            Phone = Placeholders.PhoneNumber()
        };
    }
}