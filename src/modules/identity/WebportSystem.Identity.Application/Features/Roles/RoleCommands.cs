using FluentValidation;
using Microsoft.AspNetCore.Identity;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Identity.Domain.Roles;

namespace WebportSystem.Identity.Application.Features.Roles;

#region Create
public sealed record CreateRoleCommand(string RoleName) : ICommand;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}

public class CreateRoleCommandHandler(RoleManager<RoleM> roleManager)
    : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> Handle(
        CreateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var role = new RoleM
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
#endregion

#region Update
public sealed record UpdateRoleCommand(string RoleId, string RoleName) : ICommand;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleId).NotEmpty();
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}
public class UpdateRoleCommandHandler(RoleManager<RoleM> roleManager)
    : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> Handle(
        UpdateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var roleToUpdate = await roleManager.FindByIdAsync(command.RoleId);

        if (roleToUpdate is null)
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
#endregion

#region Delete
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
#endregion