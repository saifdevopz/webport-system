using FluentValidation;

namespace WebportSystem.Inventory.Application.Features.Item;

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

public sealed record UpdateItemCommand(int ItemId, string ItemDesc) : ICommand;

public sealed record UpdateItemResult(ItemM Result);

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(_ => _.ItemId).NotEmpty();
        RuleFor(_ => _.ItemDesc).NotEmpty();
    }
}