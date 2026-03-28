using WebportSystem.Common.Domain.Contracts.Identity;

namespace WebportSystem.Identity.Application.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(AccessTokenDto request);
    Task<Result<TokenResponse>> RefreshToken(RefreshTokenDto request);
}
