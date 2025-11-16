using WebportSystem.Common.Domain.Results;
using WebportSystem.Identity.Application.Dtos;

namespace WebportSystem.Identity.Application.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(AccessTokenRequest request);
    Task<Result<TokenResponse>> RefreshToken(RefreshTokenRequest request);
}
