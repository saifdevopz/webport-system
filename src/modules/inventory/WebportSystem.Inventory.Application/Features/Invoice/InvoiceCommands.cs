using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Inventory.Domain.Entities.Invoice;

namespace WebportSystem.Inventory.Application.Features.Invoice;

public sealed record CreateInvoiceCommand(
    DateTime? InvoiceDate,
    DateTime? DueDate,
    int CustomerId,
    string Notes,
    List<CreateInvoiceItem> Items)
: ICommand<int>;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(_ => _.CustomerId).NotEmpty();

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
        var invoice = InvoiceM.Create(
            DateOnly.FromDateTime(command.InvoiceDate ?? DateTime.Today),
            DateOnly.FromDateTime(command.DueDate ?? DateTime.Today),
            command.CustomerId,
            command.Notes);

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
    DateTime? InvoiceDate,
    DateTime? DueDate,
    int CustomerId,
    string Notes,
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

        record.UpdateInvoice(
            DateOnly.FromDateTime(command.InvoiceDate ?? DateTime.Today),
            DateOnly.FromDateTime(command.DueDate ?? DateTime.Today),
            command.CustomerId,
            command.Notes);

        var items = command.Items.Select(i =>
        (
            itemId: i.ItemId,
            name: i.ItemDesc,
            price: i.UnitPrice,
            qty: (int)i.Quantity
        )).ToList();

        record.ReplaceItems(items);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(record);
    }
}

