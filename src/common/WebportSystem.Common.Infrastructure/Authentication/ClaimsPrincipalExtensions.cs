using System.Security.Claims;
using WebportSystem.Common.Contracts.Identity;
using WebportSystem.Common.Contracts.Shared.Exceptions;

namespace WebportSystem.Common.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static int GetTenantId(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            throw new CustomException("Claims Principal Error");

        var tenantValue = principal.FindFirst(CustomClaims.TenantId)!.Value;

        return int.TryParse(tenantValue, out var tenantId)
            ? tenantId
            : throw new CustomException("TenantId Error");
    }

    public static string GetUserEmail(this ClaimsPrincipal? principal)
    {
        var email = principal?.FindFirst(CustomClaims.Email)?.Value;

        if (string.IsNullOrWhiteSpace(email))
            return "System Admin";

        return email;
    }

    public static string GetTenantDatabaseName(this ClaimsPrincipal? principal)
    {
        var databaseName = principal?.FindFirst(CustomClaims.DatabaseName)?.Value;

        return databaseName!;
    }


    public static int GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirst(CustomClaims.UserId)?.Value;

        return int.TryParse(userId, out int parsedUserId) ?
        parsedUserId :
            throw new CustomException("User identifier is unavailable");
    }

    public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
    {
        IEnumerable<Claim> permissionClaims = principal?.FindAll(CustomClaims.Permission) ??
                                              throw new CustomException("Permissions are unavailable");

        return [.. permissionClaims.Select(c => c.Value)];
    }
}
