using System.Security.Claims;
using WebportSystem.Common.Domain.Contracts.Identity;
using WebportSystem.Dashboard.Common.Services.Interfaces;

namespace WebportSystem.Dashboard.Common.Services.Implementations;

public sealed class TenantContext : ITenantContext
{
    public string? TenantId { get; private set; }
    public string? UserId { get; private set; }

    public string? Email { get; private set; }
    public string? Role { get; private set; }
    public string? AccessToken { get; private set; }
    public ClaimsPrincipal? User { get; private set; }


    public void InitializeFromUser(ClaimsPrincipal user)
    {
        User = user;

        TenantId = user.FindFirst(CustomClaims.TenantId)?.Value;

        UserId = user.FindFirst(CustomClaims.UserId)?.Value;

        Email = user.FindFirst(CustomClaims.Email)?.Value;

        Role = user.FindFirst(ClaimTypes.Role)?.Value;

        AccessToken = user.FindFirst("access_token")?.Value;
    }
}