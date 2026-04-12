using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Common.Application.Authorization;

public interface IPermissionService
{
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(int userId);
}