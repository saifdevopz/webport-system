using WebportSystem.Common.Domain.Contracts.Identity;
using WebportSystem.Common.Domain.Results;

namespace WebportSystem.Dashboard.Common.Services.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(LoginDto request, CancellationToken cancellationToken = default);
    Task<TokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}