using FluentValidation;

namespace WebportSystem.Inventory.Application.Features.Item;

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

public sealed record DeleteItemCommand(int ItemId) : ICommand;

public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
{
    public DeleteItemCommandValidator()
    {
        RuleFor(_ => _.ItemId).NotEmpty();
    }
}