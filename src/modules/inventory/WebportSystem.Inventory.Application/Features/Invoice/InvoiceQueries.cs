using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Invoice;

public sealed record GetInvoiceByIdQuery(int InvoiceId) : IQuery<GetInvoiceByIdQueryResult>;

public sealed record GetInvoiceByIdQueryResult(InvoiceDto Record);

public class GetInvoiceByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetInvoiceByIdQuery, GetInvoiceByIdQueryResult>
{
    public async Task<Result<GetInvoiceByIdQueryResult>> Handle(
        GetInvoiceByIdQuery query,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Invoices
            .Include(_ => _.Items)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.InvoiceId == query.InvoiceId, cancellationToken);

        if (record is null)
            return Result.Failure<GetInvoiceByIdQueryResult>(
                CustomError.NotFound(nameof(GetInvoiceByIdQueryHandler), "Record not found."));

        var dto = new InvoiceDto
        {
            InvoiceId = record.InvoiceId,
            InvoiceNumber = record.InvoiceNumber,
            BusinessProfileId = record.BusinessProfileId,
            CustomerId = record.CustomerId,
            SubTotal = record.SubTotal,
            Total = record.Total,
            Items = [.. record.Items.Select(_ => new InvoiceItemDto
            {
                ItemId = _.ItemId,
                ItemName = _.ItemName,
                UnitPrice = _.UnitPrice,
                Quantity = _.Quantity,
                Total = _.Total
            })]
        };

        return Result.Success(new GetInvoiceByIdQueryResult(dto));
    }
}

public sealed record GetInvoicesQuery : IQuery<GetInvoicesQueryResult>;

public sealed record GetInvoicesQueryResult(IEnumerable<InvoiceDto> Records);

public class GetInvoicesQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetInvoicesQuery, GetInvoicesQueryResult>
{
    public async Task<Result<GetInvoicesQueryResult>> Handle(
        GetInvoicesQuery query,
        CancellationToken cancellationToken)
    {
        var records = await dbContext.Invoices
            .AsNoTracking()
            .Select(_ => new InvoiceDto
            {
                InvoiceId = _.InvoiceId,
                InvoiceNumber = _.InvoiceNumber,
                BusinessProfileId = _.BusinessProfileId,
                CustomerId = _.CustomerId,
                SubTotal = _.SubTotal,
                Total = _.Total
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return Result.Success(new GetInvoicesQueryResult(records));
    }
}

public sealed record GetInvoicePrintQuery(int InvoiceId) : IQuery<GetInvoicePrintQueryResult>;

public sealed record GetInvoicePrintQueryResult(InvoicePrintDto Records);

public class GetInvoicePrintQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetInvoicePrintQuery, GetInvoicePrintQueryResult>
{
    public async Task<Result<GetInvoicePrintQueryResult>> Handle(
        GetInvoicePrintQuery query,
        CancellationToken cancellationToken)
    {
        InvoicePrintDto? invoice = await dbContext.Invoices
                .AsNoTracking()
                .Where(x => x.InvoiceId == query.InvoiceId)
                .Select(x => new InvoicePrintDto
                {
                    InvoiceNumber = x.InvoiceNumber,
                    BusinessName = x.BusinessProfile.BusinessName,
                    BusinessAddress = $"{x.BusinessProfile.AddressLine1}, {x.BusinessProfile.City}, {x.BusinessProfile.City}",
                    CustomerName = x.Customer!.Name,
                    CustomerAddress = $"{x.Customer.Province}, {x.Customer.City}, {x.Customer.City}",
                    SubTotal = x.SubTotal,
                    Total = x.Total,
                    Items = x.Items.Select(i => new InvoiceItemDto
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        UnitPrice = i.UnitPrice,
                        Quantity = i.Quantity,
                        Total = i.Total
                    }).ToList()
                })
                .SingleOrDefaultAsync(cancellationToken);

        if (invoice == null)
            return Result.Failure<GetInvoicePrintQueryResult>(CustomError.NotFound(nameof(GetInvoicePrintQueryResult), "Invoice not found."));

        return Result.Success(new GetInvoicePrintQueryResult(invoice));
    }
}

//public async Task<Result<InvoicePrintDto>> Handle(
//    GetInvoicePrintQuery request,
//    CancellationToken cancellationToken)
//{
//    var invoice = await dbContext.Invoices
//            .AsNoTracking()
//            .Where(x => x.InvoiceId == request.InvoiceId)
//            .Select(x => new InvoicePrintDto
//            {
//                InvoiceNumber = x.InvoiceNumber,
//                BusinessName = x.BusinessProfile.BusinessName,
//                BusinessAddress = $"{x.BusinessProfile.AddressLine1}, {x.BusinessProfile.City}, {x.BusinessProfile.City}",
//                CustomerName = x.Customer!.Name,
//                CustomerAddress = $"{x.Customer.Province}, {x.Customer.City}, {x.Customer.City}",
//                SubTotal = x.SubTotal,
//                Total = x.Total,
//                Items = x.Items.Select(i => new InvoiceItemDto
//                {
//                    ItemId = i.ItemId,
//                    ItemName = i.ItemName,
//                    UnitPrice = i.UnitPrice,
//                    Quantity = i.Quantity,
//                    Total = i.Total
//                }).ToList()
//            })
//            .SingleOrDefaultAsync(cancellationToken);

//    if (invoice == null)
//        return Result.Failure<InvoicePrintDto>(CustomError.NotFound(nameof(GetInvoicePrintQuery), "Invoice not found."));

//    return Result.Success(invoice);
//}
//}