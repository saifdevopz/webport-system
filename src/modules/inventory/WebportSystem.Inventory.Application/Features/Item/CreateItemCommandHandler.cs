using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Item;

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