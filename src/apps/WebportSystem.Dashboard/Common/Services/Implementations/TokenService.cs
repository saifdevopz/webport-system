using Microsoft.AspNetCore.Mvc;
using WebportSystem.Common.Contracts.Identity;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Dashboard.Common.HttpClients;
using WebportSystem.Dashboard.Common.Services.Interfaces;


namespace WebportSystem.Dashboard.Common.Services.Implementations;

public class TokenService(BaseHttpClient httpClient) : ITokenService
{
    public async Task<Result<TokenResponse>> AccessToken(LoginDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            HttpClient httpclient = httpClient.GetPublicHttpClient();
            var response = await httpclient.PostAsJsonAsync($"/token/access", request, cancellationToken);

            if (response == null)
                return Result.Failure<TokenResponse>(CustomError.Conflict("NETWORK", "No response from server."));

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>(cancellationToken);

                if (result == null || result.IsFailure)
                    return Result.Failure<TokenResponse>(CustomError.Conflict("RESPONSE", "Unexpected empty or failed result."));

                return Result.Success(result.Data);
            }

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);

            if (problem != null)
            {
                return Result.Failure<TokenResponse>(CustomError.Failure(problem.Title!, problem.Detail!));
            }

            return Result.Failure<TokenResponse>(CustomError.Conflict("HTTP", $"Unexpected error: {response.StatusCode}"));
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<TokenResponse>(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }

    public Task<TokenResponse> RefreshToken(RefreshTokenDto request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
