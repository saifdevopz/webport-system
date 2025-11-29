using FluentValidation;
using Microsoft.AspNetCore.Identity;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Application.Features.Roles;

public class UpdateRoleCommandHandler(RoleManager<IdentityRole> roleManager)
    : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> Handle(
        UpdateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var roleToUpdate = await roleManager.FindByIdAsync(command.RoleId);

        if (roleToUpdate == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Role not found."));
        }

        roleToUpdate.Name = command.RoleName;
        
        var result = await roleManager.UpdateAsync(roleToUpdate);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result.Failure(CustomError.Problem("Bad Request", errors));
        }

        return Result.Success();
    }
}

public sealed record UpdateRoleCommand(string RoleId, string RoleName) : ICommand;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleId).NotEmpty();
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}