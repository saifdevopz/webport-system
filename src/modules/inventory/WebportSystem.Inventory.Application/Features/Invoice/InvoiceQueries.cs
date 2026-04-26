using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;

namespace WebportSystem.Inventory.Application.Features.Invoice;

public sealed record GetInvoiceByIdQuery(int InvoiceId) : IQuery<InvoiceDto>;

public class GetInvoiceByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetInvoiceByIdQuery, InvoiceDto>
{
    public async Task<Result<InvoiceDto>> Handle(
        GetInvoiceByIdQuery query,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Invoices
            .Include(_ => _.Items)
            .Include(_ => _.Customer)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.InvoiceId == query.InvoiceId, cancellationToken);

        if (record is null)
            return Result.Failure<InvoiceDto>(
                CustomError.NotFound(nameof(GetInvoiceByIdQueryHandler), "Record not found."));

        var dto = new InvoiceDto
        {
            InvoiceId = record.InvoiceId,
            CustomerId = record.CustomerId,
            SubTotal = record.SubTotal,
            Total = record.Total,
            Notes = record.Notes,
            Items = [.. record.Items.Select(_ => new InvoiceItemDto
            {
                ItemId = _.ItemId,
                ItemDesc = _.ItemDesc,
                UnitPrice = _.UnitPrice,
                Quantity = _.Quantity,
                Total = _.Total
            })]
        };

        return Result.Success(dto);
    }
}

public sealed record GetInvoicesQuery : IQuery<List<InvoiceDto>>;

public class GetInvoicesQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetInvoicesQuery, List<InvoiceDto>>
{
    public async Task<Result<List<InvoiceDto>>> Handle(
        GetInvoicesQuery query,
        CancellationToken cancellationToken)
    {
        var records = await dbContext.Invoices
            .AsNoTracking()
            .Select(_ => new InvoiceDto
            {
                InvoiceId = _.InvoiceId,
                CustomerId = _.CustomerId,
                SubTotal = _.SubTotal,
                Total = _.Total
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return Result.Success(records);
    }
}

public sealed record GetInvoicePrintQuery(int InvoiceId) : IQuery<InvoicePrintDto>;

public class GetInvoicePrintQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetInvoicePrintQuery, InvoicePrintDto>
{
    public async Task<Result<InvoicePrintDto>> Handle(
        GetInvoicePrintQuery query,
        CancellationToken cancellationToken)
    {
        BusinessProfileM? businessProfile = await dbContext.BusinessProfiles.FirstOrDefaultAsync(cancellationToken);

        InvoicePrintDto? invoice = await dbContext.Invoices
                .AsNoTracking()
                .Where(x => x.InvoiceId == query.InvoiceId)
                .Select(_ => new InvoicePrintDto
                {
                    // Invoice Details
                    InvoiceId = _.InvoiceId,
                    InvoiceDate = _.InvoiceDate,
                    DueDate = _.DueDate,
                    Notes = _.Notes,

                    // Business Details
                    LogoUrl = businessProfile!.LogoUrl ?? "https://webport-pull-zone.b-cdn.net/default-logo.png",
                    BusinessName = businessProfile!.BusinessName,
                    BusinessAddress = businessProfile!.AddressLine1,
                    BusinessPostalCode = businessProfile!.PostalCode,
                    BusinessCity = businessProfile!.City,
                    BusinessProvince = businessProfile.Province,
                    BusinessPhoneNumber = businessProfile.Phone,

                    // Customer Details
                    CustomerName = _.Customer!.Name,
                    CustomerBusinessName = _.Customer.CompanyName,
                    CustomerAddress = $"{_.Customer.Province}, {_.Customer.City}, {_.Customer.City}",

                    // Item Details
                    SubTotal = _.SubTotal,
                    Total = _.Total,
                    Items = _.Items.Select(i => new InvoiceItemDto
                    {
                        ItemId = i.ItemId,
                        ItemDesc = i.ItemDesc,
                        UnitPrice = i.UnitPrice,
                        Quantity = i.Quantity,
                        Total = i.Total
                    }).ToList()
                })
                .SingleOrDefaultAsync(cancellationToken);

        if (invoice == null)
            return Result.Failure<InvoicePrintDto>(CustomError.NotFound(nameof(InvoicePrintDto), "Invoice not found."));

        return Result.Success(invoice);
    }
}