using Microsoft.AspNetCore.Identity;

namespace WebportSystem.Identity.Domain.Users;

public class User : IdentityUser
{
    public int TenantId { get; set; }
}
