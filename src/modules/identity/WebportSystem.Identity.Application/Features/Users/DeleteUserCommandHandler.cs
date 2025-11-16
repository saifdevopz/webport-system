using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Domain.Errors;
using WebportSystem.Common.Domain.Results;
using WebportSystem.Identity.Application.Data;

namespace WebportSystem.Identity.Application.Features.Users;

public class DeleteUserCommandHandler(IUsersDbContext usersDbContext)
    : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(
        DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await usersDbContext.Users.FindAsync([command.UserId], cancellationToken);

        if (user is null)
        {
            return Result.Failure(CustomError.NotFound("DeleteUserCommand", "User not found."));
        }

        usersDbContext.Users.Remove(user);
        await usersDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteUserCommand(int UserId) : ICommand;