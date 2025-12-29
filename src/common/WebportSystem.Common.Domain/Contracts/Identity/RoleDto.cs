using System.ComponentModel.DataAnnotations;

namespace WebportSystem.Common.Domain.Contracts.Identity;

public class RoleDto
{
    public required string RoleId { get; set; }

    [Required]
    [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
    public string? RoleName { get; set; }
}

public class CreateRoleDto
{
    [Required]
    [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
    public string? RoleName { get; set; }
}



public record RoleWrapper<T>(T Role);
public record RolesWrapper<T>(IEnumerable<T> roles);
