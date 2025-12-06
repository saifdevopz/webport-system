using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Category;

public sealed class CreateCategoryCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<CreateCategoryCommand>
{
    public async Task<Result> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Categories
            .SingleOrDefaultAsync(_ => _.CategoryCode == command.CategoryCode, cancellationToken);

        if (record != null)
        {
            return Result.Failure<CreateCategoryCommand>(
                CustomError.Problem(nameof(CreateCategoryCommand), "Record already exist."));
        }


        var category = CategoryM.Create(command.CategoryCode, command.CategoryDesc);

        await dbContext.Categories.AddAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateCategoryCommand(string CategoryCode, string CategoryDesc) : ICommand;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryCode)
            .NotEmpty().WithMessage("Category code is required.")
            .MaximumLength(20).WithMessage("Category code must not exceed 20 characters.");

        RuleFor(x => x.CategoryDesc)
            .NotEmpty().WithMessage("Category description is required.")
            .MaximumLength(255).WithMessage("Category description must not exceed 255 characters.");
    }
}

