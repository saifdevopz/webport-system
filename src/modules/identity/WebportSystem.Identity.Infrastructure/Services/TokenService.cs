using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebportSystem.Common.Domain.Errors;
using WebportSystem.Common.Domain.Results;
using WebportSystem.Common.Infrastructure.Authentication;
using WebportSystem.Common.Infrastructure.Clock;
using WebportSystem.Identity.Application.Dtos;
using WebportSystem.Identity.Application.Interfaces;
using WebportSystem.Identity.Domain.Users;
using WebportSystem.Identity.Infrastructure.Database;

namespace WebportSystem.Identity.Infrastructure.Services;

public class TokenService(
    IOptions<JwtOptions> JwtOptions,
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    UsersDbContext usersDbContext,
    IDateTimeProvider dateTimeProvider) : ITokenService
{
    public async Task<Result<TokenResponse>> AccessToken(AccessTokenRequest request)
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

    public async Task<Result<TokenResponse>> RefreshToken(RefreshTokenRequest request)
    {
        throw new NotImplementedException("RefreshToken functionality not implemented yet.");
    }

    private async Task<Result<TokenResponse>> GenerateTokensAndUpdateUser(User user)
    {
        UserTokenClaims tokenClaims = GetAllUserDetails(user.Email!);
        string token = GenerateJwt(tokenClaims);

        var response = new TokenResponse(token, null, null);

        return Result.Success(response);
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
        List<Claim> claims =
        [
            new Claim(CustomClaims.TenantId, customClaims.TenantId.ToString(CultureInfo.CurrentCulture)),
            new Claim(CustomClaims.UserId, customClaims.UserId.ToString(CultureInfo.CurrentCulture)),
            new Claim(ClaimTypes.Email, customClaims.Email),
            new Claim(ClaimTypes.Role, customClaims.Role),
            new Claim(CustomClaims.TenantName, customClaims.TenantName),
        ];

        return claims;
    }

    //private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)

#pragma warning disable S125 // Sections of code should not be commented out
                            //{
                            //    TokenValidationParameters tokenValidationParameters = new()
                            //    {
                            //        ValidateIssuerSigningKey = true,
                            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Value.Key)),
                            //        ValidateIssuer = true,
                            //        ValidateAudience = true,
                            //        ValidAudience = JwtOptions.Value.Audience,
                            //        ValidIssuer = JwtOptions.Value.Issuer,
                            //        RoleClaimType = ClaimTypes.Role,
                            //        ClockSkew = TimeSpan.Zero,
                            //    };

    //    JwtSecurityTokenHandler tokenHandler = new();
    //    ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken? securityToken);

    //    return securityToken is not JwtSecurityToken jwtSecurityToken ||
    //        !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase)
    //        ? throw new SecurityTokenValidationException("Invalid token.")
    //        : principal;
    //}

    private UserTokenClaims GetAllUserDetails(string email)
#pragma warning restore S125 // Sections of code should not be commented out
    {
        var userDetails = usersDbContext.Users
            .Where(u => u.Email == email)
            //.Include(_ => _.Tenant)
            //.Include(_ => _.Role)
            .Select(_ => new UserTokenClaims
            {
                UserId = 2,
                Email = "_.Email",
                TenantId = 2,
                TenantName = "_.Tenant!.TenantName",
                Role = "_.Role!.RoleName"
            })
            .FirstOrDefault();

        return userDetails!;
    }
}


