using System.ComponentModel.DataAnnotations;

namespace WebportSystem.Common.Infrastructure.Authentication;

public class JwtOptions : IValidatableObject
{
    public string Key { get; init; } = null!;

    public string Issuer { get; init; } = null!;

    public string Audience { get; init; } = null!;

    public int TokenExpirationInMinutes { get; set; }

    public int RefreshTokenExpirationInDays { get; set; }

    public int ClockSkewInMinutes { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Key))
        {
            yield return new ValidationResult("No Key defined in JwtSettings config - (JwtOptions.cs)", [nameof(Key)]);
        }
    }
}
