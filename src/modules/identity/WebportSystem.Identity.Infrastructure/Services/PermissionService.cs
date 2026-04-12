using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Application.Authorization;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Identity.Application.Features.Permissions;

namespace WebportSystem.Identity.Infrastructure.Services;

internal sealed class PermissionService(
    IQueryHandler<GetPermissionsByUserIdQuery, GetPermissionsByUserIdQueryResult> handler)
    : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(int userId)
    {
        var response = await handler
            .Handle(new GetPermissionsByUserIdQuery(userId), default);

        return Result.Success(response.Data.Permissions);
    }
}