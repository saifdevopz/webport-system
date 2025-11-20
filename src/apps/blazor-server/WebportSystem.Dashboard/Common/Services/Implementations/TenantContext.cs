using System.Security.Claims;
using WebportSystem.Dashboard.Common.Services.Interfaces;

namespace WebportSystem.Dashboard.Common.Services.Implementations;

public sealed class TenantContext : ITenantContext
{
    public string? TenantId { get; private set; }
    public string? TenantName { get; private set; }
    public string? UserId { get; private set; }
    public string? Role { get; private set; }
    public string? AccessToken { get; private set; }
    public ClaimsPrincipal? User { get; private set; }

    public void InitializeFromUser(ClaimsPrincipal user)
    {
        User = user;

        TenantId = user.FindFirst("tenant_id")?.Value
                   ?? user.FindFirst("tenant")?.Value;

        UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                 ?? user.FindFirst("sub")?.Value;

        TenantName = user.FindFirst("TenantName")?.Value;

        Role = user.FindFirst(ClaimTypes.Role)?.Value;

        AccessToken = user.FindFirst("access_token")?.Value;
    }
}