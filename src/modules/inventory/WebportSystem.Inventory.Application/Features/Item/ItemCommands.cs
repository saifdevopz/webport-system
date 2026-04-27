using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Inventory.Application.Features.Item;

public sealed record CreateItemCommand(
    int CategoryId,
    string ItemCode,
    string ItemDesc,
    decimal SellingPrice,
    decimal CostPrice) : ICommand<int>;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty();
        RuleFor(_ => _.ItemCode).NotEmpty();
        RuleFor(_ => _.ItemDesc).NotEmpty();
    }
}

public class CreateItemCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<CreateItemCommand, int>
{
    public async Task<Result<int>> Handle(
        CreateItemCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Items
            .AnyAsync(_ => _.ItemCode == command.ItemCode, cancellationToken);

        if (record)
        {
            return Result.Failure<int>(
                CustomError.Problem(nameof(CreateItemCommandHandler),
                "Record already exist."));
        }

        var item = ItemM.Create(
            command.CategoryId,
            command.ItemCode,
            command.ItemDesc,
            command.SellingPrice,
            command.CostPrice);

        await dbContext.Items.AddAsync(item, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(item.ItemId);
    }
}

public sealed record UpdateItemCommand(
    int ItemId,
    int CategoryId,
    string ItemCode,
    string ItemDesc,
    decimal SellingPrice,
    decimal CostPrice)
: ICommand;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty().NotNull();
        RuleFor(_ => _.ItemCode).NotEmpty();
        RuleFor(_ => _.ItemDesc).NotEmpty();
    }
}

public class UpdateItemCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<UpdateItemCommand>
{
    public async Task<Result> Handle(
        UpdateItemCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Items
            .FindAsync([command.ItemId], cancellationToken);

        if (record == null)
        {
            return Result.Failure(
                CustomError.NotFound(nameof(UpdateItemCommand),
                "Record not found."));
        }

        record.CategoryId = command.CategoryId;
        record.ItemDesc = command.ItemDesc;
        record.SellingPrice = command.SellingPrice;
        record.CostPrice = command.CostPrice;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteItemCommand(int ItemId) : ICommand;

public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
{
    public DeleteItemCommandValidator()
    {
        RuleFor(_ => _.ItemId).NotEmpty();
    }
}

public class DeleteItemCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<DeleteItemCommand>
{
    public async Task<Result> Handle(
        DeleteItemCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Items.FindAsync([command.ItemId], cancellationToken);

        if (record is null)
        {
            return Result.Failure(CustomError.NotFound(nameof(DeleteItemCommandHandler), "Record not found."));
        }

        dbContext.Items.Remove(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}