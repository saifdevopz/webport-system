using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace WebportSystem.Common.Infrastructure.Authentication;

public sealed class TenantContext(IHttpContextAccessor httpContextAccessor, IConfiguration config)
{
    public string GetTenantConnectionString()
    {
        string activeProvider = config["Database:ActiveProvider"]
            ?? throw new ArgumentException("Missing Database:ActiveProvider in configuration.");

        string basePath = $"Database:Providers:{activeProvider}";

        string defaultDbConnection = config[$"{basePath}:TenantConnection"]
            ?? throw new ArgumentException("Missing TenantConnection in configuration.");

        if (string.IsNullOrWhiteSpace(TenantDatabaseName))
        {
            return defaultDbConnection;
        }

        string pattern = @"(?<=Database=)([^;]*)";
        string tenantConnectionString = Regex.Replace(defaultDbConnection, pattern, TenantDatabaseName!);

        return tenantConnectionString;
    }

    public int TenantId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user == null) return 1;

            return user.GetTenantId();
        }
    }

    public string? UserEmail
    {
        get { return User.GetUserEmail(); }
    }

    public string TenantDatabaseName
    {
        get { return User.GetTenantDatabaseName(); }
    }

    public string? TenantDbName
    {
        get
        {
            var httpContext = httpContextAccessor.HttpContext;

            // Prefer explicit header
            if (httpContext?.Request.Headers.TryGetValue("Tenant", out var headerTenant) == true)
                return headerTenant.ToString();

            // Fallback to user identity name
            return httpContext?.User.Identities.FirstOrDefault()?.Name;
        }
    }

    public ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;
}