using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Item;

public sealed record CreateItemCommand(int CategoryId, string ItemCode, string ItemDesc) : ICommand;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty().NotNull();
        RuleFor(_ => _.ItemCode).NotEmpty();
        RuleFor(_ => _.ItemDesc).NotEmpty();
    }
}

public class CreateItemCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<CreateItemCommand>
{
    public async Task<Result> Handle(
        CreateItemCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Items
            .SingleOrDefaultAsync(_ => _.ItemCode == command.ItemCode, cancellationToken);

        if (record != null)
        {
            return Result.Failure<CreateItemCommand>(
                CustomError.Problem(nameof(CreateItemCommand), "Record already exist."));
        }

        var item = ItemM.Create(command.CategoryId,
                                command.ItemCode,
                                command.ItemDesc);

        await dbContext.Items.AddAsync(item, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateItemCommand(int ItemId, string ItemDesc) : ICommand;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(_ => _.ItemId).NotEmpty();
        RuleFor(_ => _.ItemDesc).NotEmpty();
    }
}

public sealed record UpdateItemResult(ItemM Result);

public class UpdateItemCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<UpdateItemCommand>
{
    public async Task<Result> Handle(
        UpdateItemCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Items.FindAsync([command.ItemId], cancellationToken);

        if (record == null)
        {
            return Result.Failure<UpdateItemCommand>(
                CustomError.NotFound(nameof(UpdateItemCommand), "Record not found."));
        }

        record.ItemDesc = command.ItemDesc;

        dbContext.Items.Update(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateItemResult(record));
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