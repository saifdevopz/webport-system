using FluentValidation;
using Microsoft.AspNetCore.Identity;
using WebportSystem.Identity.Domain.Users;


namespace WebportSystem.Identity.Application.Features.Users;

public class CreateUserCommandHandler(UserManager<User> userManager)
    : ICommandHandler<CreateUserCommand>
{
    public async Task<Result> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var model = new User
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = command.TenantId,
            Email = command.Email,
            UserName = command.FullName
        };

        var result = await userManager.CreateAsync(model, command.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result.Failure(CustomError.Problem("Bad Request", errors));
        }

        var roleResult = await userManager.AddToRoleAsync(model, "Admin");

        if (!roleResult.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result.Failure(CustomError.Problem("Bad Request", errors));
        }

        return Result.Success();
    }
}

public sealed record CreateUserCommand(
    string FullName,
    string Email,
    string Password,
    int TenantId,
    int RoleId) : ICommand;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(_ => _.FullName).NotEmpty();
        RuleFor(_ => _.Email).NotEmpty().EmailAddress();
        RuleFor(_ => _.Password).NotEmpty();
        RuleFor(_ => _.TenantId).NotEmpty();
    }
}