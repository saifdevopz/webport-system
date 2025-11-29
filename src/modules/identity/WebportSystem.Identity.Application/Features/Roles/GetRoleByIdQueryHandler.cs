using Microsoft.AspNetCore.Identity;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Application.Features.Roles;

public class GetRoleByIdQueryHandler(RoleManager<IdentityRole> roleManager)
    : IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResult>
{
    public async Task<Result<GetRoleByIdQueryResult>> Handle(
        GetRoleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var model = await roleManager.FindByIdAsync(query.RoleId);

        return model is not null
            ? Result.Success(new GetRoleByIdQueryResult(model))
            : Result.Failure<GetRoleByIdQueryResult>(CustomError.NotFound("Not Found", "Role not found."));
    }
}

public sealed record GetRoleByIdQuery(string RoleId) : IQuery<GetRoleByIdQuery>;

public sealed record GetRoleByIdQueryResult(IdentityRole Role);
