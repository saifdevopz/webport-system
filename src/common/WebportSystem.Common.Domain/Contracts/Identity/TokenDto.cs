using System.ComponentModel.DataAnnotations;

namespace WebportSystem.Common.Domain.Contracts.Identity;

public class LoginDto
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public record RefreshTokenRequest(string Token, string RefreshToken);
public record TokenResponse(string Token, string RefreshToken);

public sealed record AccessTokenRequest
(
    string Email,
    string Password
);

public class RefreshTokenRequest2
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

public sealed class UserTokenClaims
{
    public required string UserId { get; set; }
    public required int TenantId { get; set; }
    public required string Email { get; set; }
    public List<string> Roles { get; set; } = [];
}