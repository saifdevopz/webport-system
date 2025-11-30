using Microsoft.AspNetCore.Identity;

namespace WebportSystem.Identity.Domain.Roles;

public class RoleClaimM : IdentityRoleClaim<string>
{
    public RoleM Role { get; set; } = null!;
}