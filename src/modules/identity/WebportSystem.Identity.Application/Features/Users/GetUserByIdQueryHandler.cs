using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Domain.Contracts.Identity;
using WebportSystem.Identity.Application.Data;

namespace WebportSystem.Identity.Application.Features.Users;

public class GetUserByIdQueryHandler(IUsersDbContext usersDbContext)
    : IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult>
{
    public async Task<Result<GetUserByIdQueryResult>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var userDto = await usersDbContext.Users
            .Where(u => u.Id == query.UserId)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Tenant)
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName!,
                Email = u.Email!,
                TenantName = u.Tenant != null ? u.Tenant.TenantName : null!,
                Roles = u.UserRoles
                    .Select(ur => new RoleDto
                    {
                        RoleId = ur.RoleId,
                        RoleName = ur.Role.Name
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return userDto is not null
            ? Result.Success(new GetUserByIdQueryResult(userDto))
            : Result.Failure<GetUserByIdQueryResult>(CustomError.NotFound("Not Found", "User not found."));

    }
}

public sealed record GetUserByIdQuery(string UserId) : IQuery<GetUserByIdQueryResult>;

public sealed record GetUserByIdQueryResult(UserDto User);