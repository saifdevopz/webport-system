using Microsoft.EntityFrameworkCore;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Application.Features.Users;

public class GetUsersQueryHandler(IUsersDbContext usersDbContext)
    : IQueryHandler<GetUsersQuery, GetUsersQueryResult>
{
    public async Task<Result<GetUsersQueryResult>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        var model = await usersDbContext.Users.ToListAsync(cancellationToken);

        return Result.Success(new GetUsersQueryResult(model));
    }
}

public sealed record GetUsersQuery : IQuery<GetUsersQueryResult>;

public sealed record GetUsersQueryResult(IEnumerable<User> Users);