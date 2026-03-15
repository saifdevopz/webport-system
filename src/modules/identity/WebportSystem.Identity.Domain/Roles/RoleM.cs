using Microsoft.AspNetCore.Identity;
using WebportSystem.Common.Domain.Abstractions;

namespace WebportSystem.Identity.Domain.Roles;

public class RoleM : IdentityRole, ISimpleEntity
{
    public ICollection<UserRoleM> UserRoles { get; set; } = [];
    public ICollection<RoleClaimM> RoleClaims { get; set; } = [];
}