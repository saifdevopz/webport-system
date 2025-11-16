using Microsoft.AspNetCore.Http;
using WebportSystem.Common.Infrastructure.Authentication;

namespace WebportSystem.Common.Infrastructure.Database;

public sealed class TenantProvider(IHttpContextAccessor httpContextAccessor)
{
    public int TenantId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;

            // Return default tenant ID when HttpContext or User is unavailable (e.g. EF CLI)
            return user == null ? 1 : user.GetTenantId();
        }
    }

    public string UserEmail
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;

            return user.GetUserEmail();
        }
    }
}