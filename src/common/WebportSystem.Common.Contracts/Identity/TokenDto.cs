using System.ComponentModel.DataAnnotations;

namespace WebportSystem.Common.Contracts.Identity;

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

public sealed record AccessTokenDto
(
    string Email,
    string Password
);

public record RefreshTokenDto(string Token, string RefreshToken);

public record TokenResponse(string Token, string RefreshToken);

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
    public Guid? TenantId { get; set; }
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string DatabaseName { get; set; }
    public List<string> Roles { get; set; } = [];
}