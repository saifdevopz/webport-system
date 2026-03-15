using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Common.Application.Abstractions;

public sealed record GenericDeleteCommand<T>(int Id) : ICommand;

public class GenericDeleteCommandHandler<T, TContext>(TContext dbContext)
    : ICommandHandler<GenericDeleteCommand<T>>
    where T : class
    where TContext : DbContext
{
    public async Task<Result> Handle(
        GenericDeleteCommand<T> command,
        CancellationToken cancellationToken)
    {
        var model = await dbContext.Set<T>().FindAsync([command.Id], cancellationToken);

        if (model is null)
            return Result.Failure(CustomError.NotFound("Not Found", $"{typeof(T).Name} not found."));

        dbContext.Set<T>().Remove(model);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}