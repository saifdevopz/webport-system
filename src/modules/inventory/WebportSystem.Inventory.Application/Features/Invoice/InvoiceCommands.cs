using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.Invoice;

namespace WebportSystem.Inventory.Application.Features.Invoice;

public sealed record CreateInvoiceCommand(
    string InvoiceNumber,
    int BusinessProfileId,
    int? CustomerId,
    List<InvoiceItemDto> Items) : ICommand;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceNumber)
                   .NotEmpty().WithMessage("Invoice number is required.")
                   .MaximumLength(50);

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Invoice must have at least one item.");

        //RuleForEach(x => x.Items).ChildRules(item =>
        //{
        //    item.RuleFor(i => i.ItemId).NotEmpty();
        //    item.RuleFor(i => i.ItemName).NotEmpty();
        //    item.RuleFor(i => i.UnitPrice).GreaterThan(0);
        //    item.RuleFor(i => i.Quantity).GreaterThan(0);
        //});
    }
}

public sealed class CreateInvoiceCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<CreateInvoiceCommand>
{
    public async Task<Result> Handle(
        CreateInvoiceCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Invoices
            .AnyAsync(_ => _.InvoiceNumber == command.InvoiceNumber, cancellationToken);

        if (record)
        {
            return Result.Failure<CreateInvoiceCommand>(
                CustomError.Problem(nameof(CreateInvoiceCommand), "Record already exist."));
        }

        var invoice = new InvoiceM(
            invoiceNumber: command.InvoiceNumber,
            businessProfileId: command.BusinessProfileId,
            customerId: command.CustomerId);

        foreach (var item in command.Items)
        {
            invoice.AddItem(
                item.ItemId,
                item.ItemName,
                item.UnitPrice,
                item.Quantity);
        }

        await dbContext.Invoices.AddAsync(invoice, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateInvoiceCommand(
    int InvoiceId,
    int? CustomerId,
    List<InvoiceItemDto> Items) : ICommand<UpdateInvoiceResult>;

public sealed record UpdateInvoiceResult(InvoiceM Result);

public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
{
    public UpdateInvoiceCommandValidator()
    {
        RuleFor(_ => _.InvoiceId).NotEmpty();
        RuleFor(_ => _.CustomerId).NotEmpty();
    }
}

public class UpdateInvoiceCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<UpdateInvoiceCommand, UpdateInvoiceResult>
{
    public async Task<Result<UpdateInvoiceResult>> Handle(
        UpdateInvoiceCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Invoices
            .Include(_ => _.Items)
               .SingleOrDefaultAsync(x => x.InvoiceId == command.InvoiceId, cancellationToken);

        if (record == null)
        {
            return Result.Failure<UpdateInvoiceResult>(
                CustomError.NotFound(nameof(UpdateInvoiceCommandHandler), "Record not found."));
        }

        // ✅ Replace items (clean + safe)
        record.ReplaceItems(
            command.Items
                .Select(x => (x.ItemId, x.ItemName, x.UnitPrice, x.Quantity))
                .ToList()
        );

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateInvoiceResult(record));
    }
}

public sealed record DeleteInvoiceCommand(int InvoiceId) : ICommand;

public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(_ => _.InvoiceId).NotEmpty();
    }
}

public class DeleteInvoiceCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<DeleteInvoiceCommand>
{
    public async Task<Result> Handle(
        DeleteInvoiceCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Invoices
            .FindAsync([command.InvoiceId], cancellationToken);

        if (record is null)
        {
            return Result.Failure(
                CustomError.NotFound(nameof(DeleteInvoiceCommandHandler), "Record not found."));
        }

        dbContext.Invoices.Remove(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}