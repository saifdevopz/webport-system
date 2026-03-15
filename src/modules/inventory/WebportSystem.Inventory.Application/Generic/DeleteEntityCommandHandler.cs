namespace WebportSystem.Inventory.Application.Generic;

public class DeleteEntityCommandHandler<TEntity>(
    IInventoryDbContext dbContext)
    : ICommandHandler<DeleteEntityCommand<TEntity>>
    where TEntity : class
{
    public async Task<Result> Handle(
        DeleteEntityCommand<TEntity> command,
        CancellationToken cancellationToken)
    {
        var set = dbContext.Set<TEntity>();

        var entity = await set.FindAsync([command.Id], cancellationToken);

        if (entity is null)
        {
            return Result.Failure(
                CustomError.NotFound(typeof(TEntity).Name, "Record not found."));
        }

        set.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteEntityCommand<TEntity>(int Id) : ICommand
    where TEntity : class;