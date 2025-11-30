namespace WebportSystem.Common.Domain.Contracts.Identity;

public class UserDto
{
    public string Id { get; set; } = null!;

    public int TenantId { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string TenantName { get; set; } = string.Empty;

    public List<RoleDto> Roles { get; set; } = [];
}

public record UserWrapper<T>(T User);
public record UsersWrapper<T>(IEnumerable<T> Users);