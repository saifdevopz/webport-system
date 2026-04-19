using System.Security.Claims;
using WebportSystem.Common.Contracts.Identity;

namespace WebportSystem.Dashboard.Common.Authentication;

public interface ITenantContext
{
    string? TenantId { get; }
    string? UserId { get; }
    string? Email { get; }
    string? Role { get; }
    string? AccessToken { get; }
    bool IsPlatform { get; }
    ClaimsPrincipal? User { get; }
    void InitializeFromUser(ClaimsPrincipal user);
}

public sealed class TenantContext : ITenantContext
{
    public string? TenantId { get; private set; }
    public string? UserId { get; private set; }

    public string? Email { get; private set; }
    public string? Role { get; private set; }
    public string? AccessToken { get; private set; }
    public string? Scope { get; private set; }

    public bool IsPlatform => Scope == "Platform";
    public ClaimsPrincipal? User { get; private set; }

    public void InitializeFromUser(ClaimsPrincipal user)
    {
        User = user;

        if (user.Identity?.IsAuthenticated != true)
            throw new InvalidOperationException("User is not authenticated");

        TenantId = user.FindFirst(CustomClaims.TenantId)?.Value;

        UserId = user.FindFirst(CustomClaims.UserId)?.Value;

        Email = user.FindFirst(CustomClaims.Email)?.Value;

        Role = user.FindFirst(ClaimTypes.Role)?.Value;

        AccessToken = user.FindFirst("access_token")?.Value;

        Scope = user.FindFirst("scope")?.Value;
    }
}