using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebportSystem.Dashboard.Common.Authentication;

public class CustomAuthenticationStateProvider(
    IHttpContextAccessor httpContextAccessor,
    ITenantContext tenantContext) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var context = httpContextAccessor.HttpContext;

        if (context == null || !context.Request.Cookies.TryGetValue(BlazorConstants.AuthCookieName, out var token))
        {
            return Task.FromResult(new AuthenticationState(_anonymous));
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Expiry check
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                return Task.FromResult(new AuthenticationState(_anonymous));
            }

            // Build ClaimsPrincipal
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            // Initialize TenantContext from JWT claims
            tenantContext.InitializeFromUser(user);

            return Task.FromResult(new AuthenticationState(user));
        }
        catch (ArgumentException)
        {
            // Malformed token or missing claims
            return Task.FromResult(new AuthenticationState(_anonymous));
        }
        catch (SecurityTokenException)
        {
            // Token validation errors
            return Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public void MarkUserAsAuthenticated(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        // Initialize TenantContext from JWT claims
        tenantContext.InitializeFromUser(user);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}