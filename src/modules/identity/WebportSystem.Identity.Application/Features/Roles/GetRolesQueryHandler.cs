using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Identity.Domain.Roles;

namespace WebportSystem.Identity.Application.Features.Roles;

public class GetRolesQueryHandler(RoleManager<RoleM> roleManager)
    : IQueryHandler<GetRolesQuery, GetRolesQueryResult>
{
    public async Task<Result<GetRolesQueryResult>> Handle(
        GetRolesQuery query,
        CancellationToken cancellationToken)
    {
        var model = await roleManager.Roles.ToListAsync(cancellationToken);

        return Result.Success(new GetRolesQueryResult(model));
    }
}

public sealed record GetRolesQuery : IQuery<GetRolesQueryResult>;

public sealed record GetRolesQueryResult(IEnumerable<IdentityRole> Roles);