
using FluentValidation;

namespace WebportSystem.Inventory.Application.Features.Category;

public class DeleteCategoryCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(
        DeleteCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Categories.FindAsync([command.CategoryId], cancellationToken);

        if (record is null)
        {
            return Result.Failure(CustomError.NotFound(nameof(DeleteCategoryCommandHandler), "Record not found."));
        }

        dbContext.Categories.Remove(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteCategoryCommand(int CategoryId) : ICommand;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty();
    }
}