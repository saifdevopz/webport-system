using FluentValidation;
using Microsoft.AspNetCore.Identity;
using WebportSystem.Identity.Domain.Roles;

namespace WebportSystem.Identity.Application.Features.Roles;

public class DeleteRoleCommandHandler(RoleManager<RoleM> roleManager)
    : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> Handle(
        DeleteRoleCommand command,
        CancellationToken cancellationToken)
    {

        RoleM? roleToDelete = await roleManager.FindByIdAsync(command.RoleId);

        if (roleToDelete == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Role not found."));
        }

        IdentityResult result = await roleManager.DeleteAsync(roleToDelete);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result.Failure(CustomError.Problem("Bad Request", errors));
        }

        return Result.Success();
    }
}

public sealed record DeleteRoleCommand(string RoleId) : ICommand;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(_ => _.RoleId).NotEmpty();
    }
}