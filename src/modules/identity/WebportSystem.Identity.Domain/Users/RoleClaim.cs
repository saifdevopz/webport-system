using Microsoft.AspNetCore.Identity;

namespace WebportSystem.Identity.Domain.Users;

public class RoleClaim : IdentityRoleClaim<string>
{
    public Role? Role { get; set; }
}
