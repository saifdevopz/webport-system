namespace WebportSystem.Identity.Application.Authorization;

public sealed record PermissionsResponse(int UserId, HashSet<string> Permissions);