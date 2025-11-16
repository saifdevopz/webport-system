using FluentValidation;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Domain.Errors;
using WebportSystem.Common.Domain.Results;
using WebportSystem.Identity.Application.Data;

namespace WebportSystem.Identity.Application.Features.Users;

public class UpdateUserCommandHandler(IUsersDbContext usersDbContext)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(
        UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await usersDbContext.Users.FindAsync([command.UserId], cancellationToken);

        if (user is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "User not found"));
        }

        user.UserName = command.FullName;

        await usersDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateUserCommand(
    int UserId,
    string FullName) : ICommand;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(_ => _.UserId).NotEmpty();
        RuleFor(_ => _.FullName).NotEmpty();
    }
}
