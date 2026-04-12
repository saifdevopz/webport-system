using Dapper;
using WebportSystem.Common.Application.Authorization;
using WebportSystem.Common.Application.Database;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Identity.Application.Features.Permissions;

public class GetPermissionsByUserIdQueryHandler(IDbConnectionFactory dbConnection)
    : IQueryHandler<GetPermissionsByUserIdQuery, GetPermissionsByUserIdQueryResult>
{
    public async Task<Result<GetPermissionsByUserIdQueryResult>> Handle(
        GetPermissionsByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        string sql =
        $"""
		    SELECT 
		        u."id" AS "{nameof(UserPermission.UserId)}",
			    u."email" AS "{nameof(UserPermission.Email)}",
		        r."name" AS "{nameof(UserPermission.RoleName)}",
			    rc."claim_value" AS "{nameof(UserPermission.Permission)}"
		    FROM identity."AspNetUsers" u
		    LEFT JOIN identity."AspNetUserRoles" ur
		        ON ur."user_id" = u."id"
		    LEFT JOIN identity."AspNetRoles" r
		        ON r."id" = ur."role_id"
		    LEFT JOIN identity."AspNetRoleClaims" rc
			    ON rc."role_id" = r."id"
			    AND rc."claim_type" = 'permissions'
		    WHERE u."id" = @UserId
		""";

        List<UserPermission> permissions = (await dbConnection.QueryAsync<UserPermission>(sql, new { query.UserId })).AsList();

        if (permissions.Count == 0)
        {
            return Result.Failure<GetPermissionsByUserIdQueryResult>(CustomError.NotFound("404", "No permissions found."));
        }

        var response = new GetPermissionsByUserIdQueryResult(
            new PermissionsResponse(permissions[0].UserId, [.. permissions.Select(p => p.Permission)]));

        return Result.Success(response);
    }
}

public sealed record GetPermissionsByUserIdQuery(int UserId) : IQuery<GetPermissionsByUserIdQuery>;

public sealed record GetPermissionsByUserIdQueryResult(PermissionsResponse Permissions);


internal sealed class UserPermission
{
    internal string UserId { get; init; } = string.Empty;
    internal string Email { get; init; } = string.Empty;
    internal string RoleName { get; init; } = string.Empty;
    internal string Permission { get; init; } = string.Empty;
}