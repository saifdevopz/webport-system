using FluentValidation;
using WebportSystem.Identity.Application.Data;

namespace WebportSystem.Identity.Application.Features.Tenants;

public class UpdateTenantCommandHandler(IUsersDbContext dbContext)
    : ICommandHandler<UpdateTenantCommand>
{
    public async Task<Result> Handle(
        UpdateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var model = await dbContext.Tenants.FindAsync([command.TenantId], cancellationToken);

        if (model == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Tenant not found"));
        }

        model.TenantName = command.TenantName;

        dbContext.Tenants.Update(model);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(model);
    }
}

public sealed record UpdateTenantCommand(
    int TenantId,
    string TenantName) : ICommand;

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(_ => _.TenantId).NotEmpty();
        RuleFor(_ => _.TenantName).NotEmpty();
    }
}