using Microsoft.AspNetCore.Identity;

namespace WebportSystem.Identity.Domain.Users;

public class UserRole : IdentityUserRole<string>
{
    public User? User { get; set; }
    public Role? Role { get; set; }
}
