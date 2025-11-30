using FluentValidation;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Domain.Tenants;

namespace WebportSystem.Identity.Application.Features.Tenants;

public class CreateTenantCommandHandler(IUsersDbContext dbContext)
    : ICommandHandler<CreateTenantCommand>
{
    public async Task<Result> Handle(
        CreateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var model = TenantM.Create(command.TenantName, command.DatabaseConnectionString);

        await dbContext.Tenants.AddAsync(model, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateTenantCommand(
    string TenantName,
    string DatabaseConnectionString) : ICommand;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(_ => _.TenantName).NotEmpty();
    }
}
