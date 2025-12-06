using FluentValidation;

namespace WebportSystem.Inventory.Application.Features.Category;

public class UpdateCategoryCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
{
    public async Task<Result<UpdateCategoryResult>> Handle(
        UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Categories.FindAsync([command.CategoryId], cancellationToken);

        if (record == null)
        {
            return Result.Failure<UpdateCategoryResult>(
                CustomError.NotFound(nameof(UpdateCategoryCommandHandler), "Record not found."));
        }

        record.CategoryDesc = command.CategoryDesc;

        dbContext.Categories.Update(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateCategoryResult(record));
    }
}

public sealed record UpdateCategoryCommand(int CategoryId, string CategoryDesc) : ICommand<UpdateCategoryResult>;

public sealed record UpdateCategoryResult(CategoryM Result);

public class UpdateRoleCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty();
        RuleFor(_ => _.CategoryDesc).NotEmpty();
    }
}