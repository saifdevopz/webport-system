namespace WebportSystem.Identity.Application.Dtos;

public sealed record TokenResponse(string Token, string? RefreshToken, DateTime? RefreshTokenExpiryTime);

public sealed record AccessTokenRequest
(
    string Email,
    string Password
);

public class RefreshTokenRequest
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}

public sealed record CustomUserClaim
(
    int UserId,
    int TenantId,
    string Email,
    string RoleName,
    string TenantTypeCode,
    string TenantName,
    string DatabaseName
);

public sealed record TokenClaimsResponse2
(
    int? UserId = null,
    int? TenantId = null,
    string? Email = null,
    HashSet<string>? RoleName = null,
    string? TenantTypeCode = null,
    string? TenantName = null,
    string? DatabaseName = null,
    string? Expiry = null
);

public sealed class UserTokenClaims
{
    public int UserId { get; set; }
    public int TenantId { get; set; }
    public required string Role { get; set; }
    public required string Email { get; set; }
    public required string TenantName { get; set; }
    public string? Expiry { get; set; }
}
