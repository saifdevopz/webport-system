using Microsoft.AspNetCore.Identity;

namespace WebportSystem.Identity.Domain.Users;

public class UserToken : IdentityUserToken<string>
{
    public User User { get; set; } = new User();
}
