using WebportSystem.Common.Contracts.Identity;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Dashboard.Common.Services.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(LoginDto request, CancellationToken cancellationToken = default);
    Task<TokenResponse> RefreshToken(RefreshTokenDto request, CancellationToken cancellationToken = default);
}