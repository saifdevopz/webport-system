using Microsoft.AspNetCore.Identity;

namespace WebportSystem.Identity.Domain.Users;

public class UserClaim : IdentityUserClaim<string>
{
    public User User { get; set; } = new User();
}
