using Microsoft.AspNetCore.Identity;

namespace WebportSystem.Identity.Domain.Users;

public class User : IdentityUser
{
    public ICollection<UserClaim> Claims { get; set; } = [];

    public ICollection<UserRole> UserRoles { get; set; } = [];

    public ICollection<UserLogin> UserLogins { get; set; } = [];

    public ICollection<UserToken> UserTokens { get; set; } = [];

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
