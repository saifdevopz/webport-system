using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Domain.Errors;
using WebportSystem.Common.Domain.Results;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Application.Features.Users;

public class GetUserByIdQueryHandler(IUsersDbContext usersDbContext)
    : IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult>
{
    public async Task<Result<GetUserByIdQueryResult>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await usersDbContext.Users.FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        return user is not null
            ? Result.Success(new GetUserByIdQueryResult(user))
            : Result.Failure<GetUserByIdQueryResult>(CustomError.NotFound("Not Found", "User not found."));

    }
}

public sealed record GetUserByIdQuery(string UserId) : IQuery<GetUserByIdQueryResult>;

public sealed record GetUserByIdQueryResult(User User);