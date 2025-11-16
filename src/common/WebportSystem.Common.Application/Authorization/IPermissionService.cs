using WebportSystem.Common.Domain.Results;

namespace WebportSystem.Identity.Application.Authorization;

public interface IPermissionService
{
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(int userId);
}