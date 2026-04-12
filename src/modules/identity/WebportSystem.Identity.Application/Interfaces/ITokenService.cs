using WebportSystem.Common.Contracts.Identity;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Identity.Application.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(AccessTokenDto request);
    Task<Result<TokenResponse>> RefreshToken(RefreshTokenDto request);
}
