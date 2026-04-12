using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Inventory.Domain.Entities.Invoice;

namespace WebportSystem.Inventory.Application.Features.Invoice;

public sealed record CreateInvoiceCommand(
    string InvoiceNumber,
    int BusinessProfileId,
    int CustomerId,
    List<CreateInvoiceItem> Items)
: ICommand<int>;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(_ => _.InvoiceNumber)
                   .NotEmpty().WithMessage("Invoice number is required.")
                   .MaximumLength(50);

        RuleFor(_ => _.Items)
            .NotEmpty().WithMessage("Invoice must have at least one item.");

        RuleForEach(_ => _.Items).ChildRules(item =>
        {
            item.RuleFor(_ => _.ItemId).NotEmpty();
            item.RuleFor(_ => _.Quantity).GreaterThan(0);
        });
    }
}

public sealed class CreateInvoiceCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<CreateInvoiceCommand, int>
{
    public async Task<Result<int>> Handle(
        CreateInvoiceCommand command,
        CancellationToken cancellationToken)
    {
        var exists = await dbContext.Invoices
            .AnyAsync(x => x.InvoiceNumber == command.InvoiceNumber, cancellationToken);

        if (exists)
        {
            return Result.Failure<int>(
                CustomError.Problem(nameof(CreateInvoiceCommand), "Record already exists."));
        }

        var invoice = new InvoiceM(
            invoiceNumber: command.InvoiceNumber,
            businessProfileId: 1,
            customerId: command.CustomerId);

        var itemIds = command.Items.Select(x => x.ItemId).ToList();

        var dbItems = await dbContext.Items
            .Where(x => itemIds.Contains(x.ItemId))
            .ToDictionaryAsync(x => x.ItemId, cancellationToken);

        foreach (var item in command.Items)
        {
            if (!dbItems.TryGetValue(item.ItemId, out var dbItem))
            {
                return Result.Failure<int>(
                    CustomError.NotFound("Item", $"Item with ID {item.ItemId} not found."));
            }

            invoice.AddItem(
                dbItem.ItemId,
                dbItem.ItemDesc,
                dbItem.SellingPrice,
                item.Quantity
            );
        }


        await dbContext.Invoices.AddAsync(invoice, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(invoice.InvoiceId);
    }
}

public sealed record UpdateInvoiceCommand(
    int InvoiceId,
    int? CustomerId,
    List<InvoiceItemDto> Items)
: ICommand;

public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
{
    public UpdateInvoiceCommandValidator()
    {
        RuleFor(_ => _.InvoiceId).NotEmpty();
        RuleFor(_ => _.CustomerId).NotEmpty();
    }
}

public class UpdateInvoiceCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<UpdateInvoiceCommand>
{
    public async Task<Result> Handle(
        UpdateInvoiceCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Invoices
            .Include(_ => _.Items)
               .SingleOrDefaultAsync(x => x.InvoiceId == command.InvoiceId, cancellationToken);

        if (record == null)
        {
            return Result.Failure(
                CustomError.NotFound(nameof(UpdateInvoiceCommandHandler),
                "Record not found."));
        }

        // ✅ Replace items (clean + safe)
        record.ReplaceItems(
            [.. command.Items.Select(x => (x.ItemId, x.ItemDesc, x.UnitPrice, x.Quantity))]
        );

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(record);
    }
}

