using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebportSystem.Common.Domain.Errors;
using WebportSystem.Common.Domain.Results;
using WebportSystem.Dashboard.Common.HttpClients;

namespace WebportSystem.Dashboard.Common.Services.Implementations;

public class DataService(BaseHttpClient BaseHttpClient)
{
    private readonly BaseHttpClient _baseHttpClient = BaseHttpClient;

    public async Task<Result<ListWrapper<T>>> GetAllAsync<T>(string source)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            HttpResponseMessage httpResponse = await client.GetAsync(new Uri(source, UriKind.RelativeOrAbsolute)).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var problem = await httpResponse.Content
                    .ReadFromJsonAsync<ProblemDetails>()
                    .ConfigureAwait(false);

                if (problem is not null)
                    return Result.Failure<ListWrapper<T>>(CustomError.Failure(problem.Title!, problem.Detail!));

                return Result.Failure<ListWrapper<T>>(CustomError.Conflict("HTTP", $"Unexpected status: {httpResponse.StatusCode}"));

            }
            else
            {
                var result = await httpResponse.Content.ReadFromJsonAsync<Result<ListWrapper<T>>>().ConfigureAwait(false);

                if (result is null)
                {
                    return Result.Failure<ListWrapper<T>>(CustomError.Conflict("Deserialization", "Invalid response format."));
                }

                return result.IsSuccess
                    ? Result.Success(result.Data!)
                    : Result.Failure<ListWrapper<T>>(CustomError.Failure("API Error", "Request failed."));
            }
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<ListWrapper<T>>(CustomError.Conflict("Exception Occured", $"{ex.Message}"));
        }
    }

    public async Task<Result<T>> GetByIdAsync<T>(string basePath, string id)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            HttpResponseMessage httpResponse = await client.GetAsync($"{basePath}/{id}").ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var problemDetails = await httpResponse.Content
                    .ReadFromJsonAsync<ProblemDetails>()
                    .ConfigureAwait(false);

                if (problemDetails is not null)
                    return Result.Failure<T>(CustomError.Failure(problemDetails.Title!, problemDetails.Detail!));

                return Result.Failure<T>(
                    CustomError.Conflict("HTTP", $"Unexpected status: {httpResponse.StatusCode}")
                );
            }
            else
            {
                var result = await httpResponse.Content
                    .ReadFromJsonAsync<Result<Wrapper<T>>>()
                    .ConfigureAwait(false);

                if (result is null)
                {
                    return Result.Failure<T>(CustomError.Conflict("Deserialization", "Invalid response format."));
                }

                return result.IsSuccess
                    ? Result.Success(result.Data.Record)
                    : Result.Failure<T>(CustomError.Failure("Error", "Request failed."));
            }

        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict("Exception Occured", $"{ex.Message}"));
        }
    }

    public async Task<Result> PostAsync<T>(string source, T obj)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync(source, obj).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var problemDetails = await httpResponse.Content
                    .ReadFromJsonAsync<ProblemDetails>()
                    .ConfigureAwait(false);

                if (problemDetails is not null)
                {
                    string errorMessage = string.Empty;

                    if (problemDetails.Extensions.TryGetValue("errors", out var errorsElement)
                        && errorsElement is JsonElement jsonElement
                        && jsonElement.ValueKind == JsonValueKind.Array)
                    {
                        // Deserialize each item in the array
                        var errors = jsonElement.EnumerateArray()
                            .Select(e => e.GetProperty("description").GetString())
                            .Where(d => !string.IsNullOrWhiteSpace(d));

                        errorMessage = string.Join(Environment.NewLine, errors);
                    }

                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        errorMessage = $"{problemDetails.Title}: {problemDetails.Detail}";
                    }

                    return Result.Failure<T>(CustomError.Failure("Validation Failed", errorMessage));
                }

                return Result.Failure<T>(CustomError.Conflict("HTTP", $"Unexpected status: {httpResponse.StatusCode}"));
            }

            var result = await httpResponse.Content
                .ReadFromJsonAsync<Result>()
                .ConfigureAwait(false);

            if (result == null)
            {
                return Result.Failure(CustomError.Conflict("Deserialization", "Response could not be parsed."));
            }

            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure(CustomError.Conflict("Exception Occurred", ex.Message));
        }
    }

    public async Task<Result> PutAsync<T>(string source, T obj)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();

            var command = new { command = obj };
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync(source, command).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var problem = await httpResponse.Content
                    .ReadFromJsonAsync<ProblemDetails>()
                    .ConfigureAwait(false);

                if (problem is not null)
                {
                    return Result.Failure(CustomError.Failure(problem.Title!, problem.Detail!));
                }

                return Result.Failure(CustomError.Conflict("HTTP", $"Unexpected error: {httpResponse.StatusCode}"));
            }

            var result = await httpResponse.Content
                .ReadFromJsonAsync<Result>()
                .ConfigureAwait(false);

            if (result is null)
            {
                return Result.Failure(CustomError.Conflict("Deserialization", "Invalid response format."));
            }

            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure(CustomError.Conflict("Exception Occurred", ex.Message));
        }
    }

    public async Task<Result> DeleteByIdAsync(string source, int id)
    {
        try
        {
            var deleteUri = new Uri($"{source}/{id}", UriKind.RelativeOrAbsolute);

            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            HttpResponseMessage httpResponse = await client.DeleteAsync(deleteUri).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var problem = await httpResponse.Content
                    .ReadFromJsonAsync<ProblemDetails>()
                    .ConfigureAwait(false);

                if (problem != null)
                {
                    return Result.Failure(CustomError.Failure(problem.Title!, problem.Detail!));
                }

                return Result.Failure(CustomError.Conflict("HTTP", $"Unexpected status: {httpResponse.StatusCode}"));
            }

            var result = await httpResponse.Content
                .ReadFromJsonAsync<Result>()
                .ConfigureAwait(false);

            if (result is null)
            {
                return Result.Failure(CustomError.Conflict("Deserialization", "Invalid response format."));
            }

            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure(CustomError.Conflict("Exception Occurred", ex.Message));
        }
    }

}
