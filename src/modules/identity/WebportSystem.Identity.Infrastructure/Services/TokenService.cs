using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebportSystem.Common.Contracts.Identity;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Common.Infrastructure.Authentication;
using WebportSystem.Common.Infrastructure.Clock;
using WebportSystem.Identity.Application.Interfaces;
using WebportSystem.Identity.Domain.Users;
using WebportSystem.Identity.Infrastructure.Database;

namespace WebportSystem.Identity.Infrastructure.Services;

public class TokenService(
    IOptions<JwtOptions> JwtOptions,
    UserManager<UserM> userManager,
    SignInManager<UserM> signInManager,
    UsersDbContext usersDbContext,
    IDateTimeProvider dateTimeProvider) : ITokenService
{
    public async Task<Result<TokenResponse>> AccessToken(AccessTokenDto request)
    {
        var userObj = await userManager.FindByEmailAsync(request.Email);

        if (userObj is null)
        {
            return Result.Failure<TokenResponse>(CustomError.NotFound("TokenService", "User not found."));
        }
        else
        {
            var result = await signInManager.CheckPasswordSignInAsync(userObj, request.Password, false);

            if (!result.Succeeded)
            {
                return Result.Failure<TokenResponse>(CustomError.NotFound("TokenService", "Invalid Credentials."));
            }
        }

        return await GenerateTokensAndUpdateUser(userObj);
    }

    public async Task<Result<TokenResponse>> RefreshToken(RefreshTokenDto request)
    {
        ClaimsPrincipal userPrincipal = GetPrincipalFromExpiredToken(request.Token);

        UserM? user = await userManager.FindByEmailAsync(userPrincipal.GetUserEmail());

        if (user is null)
        {
            return Result.Failure<TokenResponse>(CustomError.NotFound("404", "User not found."));
        }

        // Get stored token from AspNetUserTokens
        string? refreshToken = await userManager.GetAuthenticationTokenAsync(user, "JWTAuthentication", "RefreshToken");

        return refreshToken != request.RefreshToken
            ? Result.Failure<TokenResponse>(CustomError.NotFound("404", "Invalid Refresh Token."))
            : await GenerateTokensAndUpdateUser(user);
    }

    private async Task<Result<TokenResponse>> GenerateTokensAndUpdateUser(UserM user)
    {
        UserTokenClaims tokenClaims = await GetAllUserDetails(user.Id);

        string token = GenerateJwt(tokenClaims);
        string refreshToken = GenerateRefreshToken();

        await userManager.SetAuthenticationTokenAsync(user, "JWTAuthentication", "RefreshToken", refreshToken);

        return Result.Success(new TokenResponse(token, refreshToken));
    }

    private string GenerateJwt(UserTokenClaims customClaims)
    {
        return GenerateEncryptedToken(GetSigningCredentials(), GetClaims(customClaims));
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        JwtSecurityToken token = new(
             claims: claims,
             expires: dateTimeProvider.Now.AddMinutes(JwtOptions.Value.TokenExpirationInMinutes),
             signingCredentials: signingCredentials,
             issuer: JwtOptions.Value.Issuer,
             audience: JwtOptions.Value.Audience);

        JwtSecurityTokenHandler tokenHandler = new();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(JwtOptions.Value.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private static List<Claim> GetClaims(UserTokenClaims customClaims)
    {
        var claims = new List<Claim>
        {
            new(CustomClaims.TenantId, customClaims.TenantId.ToString()),
            new(CustomClaims.UserId, customClaims.UserId.ToString()),
            new(CustomClaims.Email, customClaims.Email),
            new(CustomClaims.DatabaseName, customClaims.DatabaseName)
        };

        foreach (var role in customClaims.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }


    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Value.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = JwtOptions.Value.Audience,
            ValidIssuer = JwtOptions.Value.Issuer,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
        };

        JwtSecurityTokenHandler tokenHandler = new();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken? securityToken);

        return securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase)
            ? throw new SecurityTokenValidationException("Invalid token.")
            : principal;
    }

    private async Task<UserTokenClaims> GetAllUserDetails(string userId)
    {
        var userDto = await usersDbContext.Users
            .Where(_ => _.Id == userId)
            .Include(_ => _.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(_ => _.Tenant)
            .Select(_ => new UserTokenClaims
            {
                TenantId = _.TenantId,
                UserId = _.Id,
                Email = _.Email!,
                DatabaseName = _.Tenant!.DatabaseName,
                Roles = _.UserRoles
                    .Select(ur => ur.Role.Name!)
                    .ToList(),
            })
            .FirstOrDefaultAsync();


        return userDto!;
    }

    public static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

}
