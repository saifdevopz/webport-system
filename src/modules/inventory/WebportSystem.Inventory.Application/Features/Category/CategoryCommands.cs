using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Inventory.Application.Features.Category;

public sealed record CreateCategoryCommand(
    string CategoryCode,
    string CategoryDesc
) : ICommand<int>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(_ => _.CategoryCode).NotEmpty().MaximumLength(20);
        RuleFor(_ => _.CategoryDesc).NotEmpty().MaximumLength(255);
    }

    public sealed class CreateCategoryCommandHandler(IInventoryDbContext dbContext)
        : ICommandHandler<CreateCategoryCommand, int>
    {
        public async Task<Result<int>> Handle(
            CreateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            var record = await dbContext.Categories
                .AnyAsync(_ => _.CategoryCode == command.CategoryCode, cancellationToken);

            if (record)
            {
                return Result.Failure<int>(
                    CustomError.Problem(nameof(CreateCategoryCommand),
                    "Record already exist."));
            }

            var category = CategoryM.Create(
                command.CategoryCode,
                command.CategoryDesc);

            await dbContext.Categories.AddAsync(category, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(category.CategoryId);
        }
    }
    public sealed record UpdateCategoryCommand(
        int CategoryId,
        string CategoryDesc)
    : ICommand;

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(_ => _.CategoryId).GreaterThan(0);
            RuleFor(_ => _.CategoryDesc).NotEmpty();
        }
    }

    public class UpdateCategoryCommandHandler(IInventoryDbContext dbContext)
        : ICommandHandler<UpdateCategoryCommand>
    {
        public async Task<Result> Handle(
            UpdateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            var record = await dbContext.Categories
                    .FindAsync([command.CategoryId], cancellationToken);

            if (record == null)
            {
                return Result.Failure(
                    CustomError.NotFound(nameof(UpdateCategoryCommandHandler),
                    "Record not found."));
            }

            record.CategoryDesc = command.CategoryDesc;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}