using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Identity.Domain.Roles;

namespace WebportSystem.Identity.Application.Features.Roles;

#region Get by id

public sealed record GetRoleByIdQuery(string Id) : IQuery<GetRoleByIdQueryResult>;

public sealed record GetRoleByIdQueryResult(IdentityRole Role);

public class GetRoleByIdQueryHandler(RoleManager<RoleM> roleManager)
    : IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResult>
{
    public async Task<Result<GetRoleByIdQueryResult>> Handle(
        GetRoleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var model = await roleManager.FindByIdAsync(query.Id);

        return model is not null
            ? Result.Success(new GetRoleByIdQueryResult(model))
            : Result.Failure<GetRoleByIdQueryResult>(CustomError.NotFound("Not Found", "Role not found."));
    }
}

#endregion

#region Get list

public sealed record GetRolesQuery : IQuery<GetRolesQueryResult>;

public sealed record GetRolesQueryResult(IEnumerable<IdentityRole> Roles);

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

#endregion