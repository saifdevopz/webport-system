using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Domain.Contracts.Identity;
using WebportSystem.Identity.Application.Data;

namespace WebportSystem.Identity.Application.Features.Users;

public class GetUsersQueryHandler(IUsersDbContext usersDbContext)
    : IQueryHandler<GetUsersQuery, GetUsersQueryResult>
{
    public async Task<Result<GetUsersQueryResult>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        var model = await usersDbContext.Users
              .Include(u => u.UserRoles)
                  .ThenInclude(ur => ur.Role)
              .Include(u => u.Tenant)
              .Select(u => new UserDto
              {
                  Id = u.Id,
                  Email = u.Email!,
                  TenantName = u.Tenant!.TenantName,
                  Roles = u.UserRoles
                        .Select(ur => new RoleDto
                        {
                            RoleId = ur.RoleId,
                            RoleName = ur.Role.Name
                        })
                        .ToList()
              })
              .ToListAsync(cancellationToken);

        return Result.Success(new GetUsersQueryResult(model));
    }
}

public sealed record GetUsersQuery : IQuery<GetUsersQueryResult>;

public sealed record GetUsersQueryResult(IEnumerable<UserDto> Users);
