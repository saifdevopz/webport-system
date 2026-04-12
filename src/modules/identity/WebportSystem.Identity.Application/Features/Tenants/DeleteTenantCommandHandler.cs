using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Identity.Application.Data;

namespace WebportSystem.Identity.Application.Features.Tenants;

public class DeleteTenantCommandHandler(IUsersDbContext dbContext)
    : ICommandHandler<DeleteTenantCommand>
{
    public async Task<Result> Handle(
        DeleteTenantCommand command,
        CancellationToken cancellationToken)
    {
        var model = await dbContext.Tenants.FindAsync([command.TenantId], cancellationToken);

        if (model is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Tenant not found."));
        }

        dbContext.Tenants.Remove(model);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteTenantCommand(int TenantId) : ICommand;