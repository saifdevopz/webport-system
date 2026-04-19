namespace WebportSystem.Identity.Domain.Platform;

public sealed class PlatformUserM : AggregateRoot
{
    public Guid Id { get; private set; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public bool IsSuperAdmin { get; set; } = false;
    public string? DisplayName { get; private set; }

    public static PlatformUserM Create(
        string email,
        string password,
        string? displayName)
    {
        PlatformUserM user = new()
        {
            Id = Guid.NewGuid(),
            Email = email.Trim().ToLowerInvariant(),
            Password = password,
            DisplayName = displayName
        };

        return user;
    }
}