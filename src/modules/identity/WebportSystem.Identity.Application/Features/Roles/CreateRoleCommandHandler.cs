using FluentValidation;
using Microsoft.AspNetCore.Identity;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Application.Features.Roles;

public class CreateRoleCommandHandler(RoleManager<IdentityRole> roleManager)
    : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> Handle(
        CreateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var role = new IdentityRole
        {
            Name = command.RoleName            
        };

        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result.Failure(CustomError.Problem("Bad Request", errors));
        }

        return Result.Success();
    }
}

public sealed record CreateRoleCommand(string RoleName) : ICommand;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}