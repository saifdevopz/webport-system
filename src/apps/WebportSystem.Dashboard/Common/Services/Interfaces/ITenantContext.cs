using System.Security.Claims;

namespace WebportSystem.Dashboard.Common.Services.Interfaces;

public interface ITenantContext
{
    string? TenantId { get; }
    string? UserId { get; }
    string? Email { get; }
    string? Role { get; }
    string? AccessToken { get; }
    ClaimsPrincipal? User { get; }
    void InitializeFromUser(ClaimsPrincipal user);
}