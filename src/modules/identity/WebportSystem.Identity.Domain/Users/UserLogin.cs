using Microsoft.AspNetCore.Identity;

namespace WebportSystem.Identity.Domain.Users;

public class UserLogin : IdentityUserLogin<string>
{
    public User? User { get; set; }
}
