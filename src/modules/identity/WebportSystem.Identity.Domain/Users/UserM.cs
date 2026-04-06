using Microsoft.AspNetCore.Identity;
using WebportSystem.Identity.Domain.Roles;
using WebportSystem.Identity.Domain.Tenants;

namespace WebportSystem.Identity.Domain.Users;

public class UserM : IdentityUser
{
    public Guid TenantId { get; set; }
    public TenantM? Tenant { get; set; }

    public ICollection<UserRoleM> UserRoles { get; set; } = [];
}
