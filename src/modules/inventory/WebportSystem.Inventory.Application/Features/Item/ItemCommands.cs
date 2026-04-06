using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Item;

public sealed record CreateItemCommand(ItemDto Command) : ICommand;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(_ => _.Command.CategoryId).NotEmpty().NotNull();
        RuleFor(_ => _.Command.ItemCode).NotEmpty();
        RuleFor(_ => _.Command.ItemDesc).NotEmpty();
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
            .SingleOrDefaultAsync(_ => _.ItemCode == command.Command.ItemCode, cancellationToken);

        if (record != null)
        {
            return Result.Failure<CreateItemCommand>(
                CustomError.Problem(nameof(CreateItemCommand), "Record already exist."));
        }

        var item = ItemM.Create(command.Command.CategoryId,
                                command.Command.ItemCode,
                                command.Command.ItemDesc);

        await dbContext.Items.AddAsync(item, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateItemCommand(ItemDto Command) : ICommand;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(_ => _.Command.CategoryId).NotEmpty().NotNull();
        RuleFor(_ => _.Command.ItemCode).NotEmpty();
        RuleFor(_ => _.Command.ItemDesc).NotEmpty();
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
        var record = await dbContext.Items.FindAsync([command.Command.ItemId], cancellationToken);

        if (record == null)
        {
            return Result.Failure<UpdateItemCommand>(
                CustomError.NotFound(nameof(UpdateItemCommand), "Record not found."));
        }

        record.CategoryId = command.Command.CategoryId;
        record.ItemDesc = command.Command.ItemDesc;

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