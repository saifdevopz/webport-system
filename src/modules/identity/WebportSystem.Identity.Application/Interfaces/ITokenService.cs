using WebportSystem.Common.Domain.Contracts.Identity;

namespace WebportSystem.Identity.Application.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(AccessTokenRequest request);
    Task<Result<TokenResponse>> RefreshToken(Common.Domain.Contracts.Identity.RefreshTokenRequest request);
}
