using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Category;

#region Create
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
#endregion

#region Update
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
#endregion

#region Delete
public sealed record DeleteCategoryCommand(int CategoryId) : ICommand;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty();
    }
}

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
#endregion