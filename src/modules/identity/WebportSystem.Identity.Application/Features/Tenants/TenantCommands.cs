using FluentValidation;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Domain.Tenants;

namespace WebportSystem.Identity.Application.Features.Tenants;

public sealed record CreateTenantCommand(
    string TenantName,
    string DatabaseName) : ICommand;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(_ => _.TenantName).NotEmpty();
        RuleFor(_ => _.DatabaseName).NotEmpty();
    }
}

public class CreateTenantCommandHandler(IUsersDbContext dbContext)
    : ICommandHandler<CreateTenantCommand>
{
    public async Task<Result> Handle(
        CreateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var model = TenantM.Create(command.TenantName, command.DatabaseName);

        await dbContext.Tenants.AddAsync(model, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
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

        dbContext.Tenants.Update(model);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(model);
    }
}

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