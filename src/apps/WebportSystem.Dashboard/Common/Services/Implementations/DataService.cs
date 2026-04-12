using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Dashboard.Common.HttpClients;

namespace WebportSystem.Dashboard.Common.Services.Implementations;

public class DataService(BaseHttpClient BaseHttpClient)
{
    private readonly BaseHttpClient _baseHttpClient = BaseHttpClient;

    public async Task<Result<List<T>>> GetAllAsync<T>(string source)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            HttpResponseMessage httpResponse = await client.GetAsync(new Uri(source, UriKind.RelativeOrAbsolute)).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
                return await HandleErrorResponseAsync<List<T>>(httpResponse);

            var result = await httpResponse.Content
                .ReadFromJsonAsync<Result<List<T>>>()
                .ConfigureAwait(false);

            if (result is null)
                return Result.Failure<List<T>>(CustomError.Conflict("Deserialization", "Invalid response format."));

            return result.IsSuccess
                ? Result.Success(result.Data!)
                : Result.Failure<List<T>>(result.Error!);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<List<T>>(CustomError.Conflict("Exception Occured", $"{ex.Message}"));
        }
    }

    public async Task<Result<T>> GetByIdAsync<T>(string basePath, string id)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            HttpResponseMessage httpResponse = await client.GetAsync($"{basePath}/{id}").ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
                return await HandleErrorResponseAsync<T>(httpResponse);

            var result = await httpResponse.Content
                .ReadFromJsonAsync<Result<T>>()
                .ConfigureAwait(false);

            if (result is null)
                return Result.Failure<T>(CustomError.Conflict("Deserialization", "Invalid response format."));

            return result.IsSuccess
                ? Result.Success(result.Data)
                : Result.Failure<T>(result.Error!);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict("Exception Occured", ex.Message));
        }
    }

    public async Task<Result> PostAsync<T>(string source, T obj)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync(source, obj).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
                return await HandleErrorResponseAsync(httpResponse);

            var result = await httpResponse.Content
                .ReadFromJsonAsync<Result>()
                .ConfigureAwait(false);

            if (result is null)
                return Result.Failure(CustomError.Conflict("Deserialization", "Response could not be parsed."));

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
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync(source, obj).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
                return await HandleErrorResponseAsync(httpResponse);

            var result = await httpResponse.Content
                .ReadFromJsonAsync<Result>()
                .ConfigureAwait(false);

            if (result is null)
                return Result.Failure(CustomError.Conflict("Deserialization", "Invalid response format."));

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
                return await HandleErrorResponseAsync(httpResponse);

            var result = await httpResponse.Content
                .ReadFromJsonAsync<Result>()
                .ConfigureAwait(false);

            if (result is null)
                return Result.Failure(CustomError.Conflict("Deserialization", "Invalid response format."));

            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure(CustomError.Conflict("Exception Occurred", ex.Message));
        }
    }

    // 🔥 CORE (single source of truth)
    private static async Task<CustomError> ExtractErrorAsync(HttpResponseMessage httpResponse)
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
                var errors = jsonElement.EnumerateArray()
                    .Select(e => e.GetProperty("description").GetString())
                    .Where(d => !string.IsNullOrWhiteSpace(d));

                errorMessage = string.Join(Environment.NewLine, errors);
            }

            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                errorMessage = $"{problemDetails.Title}: {problemDetails.Detail}";
            }

            return CustomError.Failure("Validation Failed", errorMessage);
        }

        return CustomError.Conflict("HTTP", $"Unexpected status: {httpResponse.StatusCode}");
    }

    // 🔹 Wrapper (non-generic)
    private static async Task<Result> HandleErrorResponseAsync(HttpResponseMessage response)
    {
        var error = await ExtractErrorAsync(response);
        return Result.Failure(error);
    }

    // 🔹 Wrapper (generic)
    private static async Task<Result<T>> HandleErrorResponseAsync<T>(HttpResponseMessage response)
    {
        var error = await ExtractErrorAsync(response);
        return Result.Failure<T>(error);
    }

}
