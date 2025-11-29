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
            UserName = command.Name
        };

        var result = await userManager.CreateAsync(model, command.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result.Failure(CustomError.Problem("Bad Request", errors));
        }

        if (!string.IsNullOrWhiteSpace(command.Role))
        {
            var roleResult = await userManager.AddToRoleAsync(model, "Admin");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return Result.Failure(CustomError.Problem("Bad Request", errors));
            }
        }

        return Result.Success();
    }
}

public sealed record CreateUserCommand(
    int TenantId,
    string Name,
    string Email,
    string Password,
    string? Role) : ICommand;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(_ => _.TenantId).NotEmpty();
        RuleFor(_ => _.Name).NotEmpty();
        RuleFor(_ => _.Email).NotEmpty().EmailAddress();
        RuleFor(_ => _.Password).NotEmpty().MinimumLength(6);
    }
}