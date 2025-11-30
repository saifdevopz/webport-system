using Microsoft.AspNetCore.Identity;
using WebportSystem.Identity.Domain.Users;

namespace WebportSystem.Identity.Domain.Roles;

public class UserRoleM : IdentityUserRole<string>
{
    public UserM User { get; set; } = null!;
    public RoleM Role { get; set; } = null!;
}
