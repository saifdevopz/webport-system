using Microsoft.AspNetCore.Identity;

namespace WebportSystem.Identity.Domain.Roles;

public class RoleM : IdentityRole
{
    public ICollection<UserRoleM> UserRoles { get; set; } = [];
    public ICollection<RoleClaimM> RoleClaims { get; set; } = [];
}