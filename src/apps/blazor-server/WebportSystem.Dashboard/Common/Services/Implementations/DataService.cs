using Microsoft.AspNetCore.Mvc;
using WebportSystem.Common.Domain.Errors;
using WebportSystem.Common.Domain.Results;
using WebportSystem.Dashboard.Common.HttpClients;


namespace WebportSystem.Dashboard.Common.Services.Implementations;

public class DataService(BaseHttpClient BaseHttpClient, TenantHttpClient TenantHttpClient)
{
    private readonly BaseHttpClient _baseHttpClient = BaseHttpClient;
    private readonly TenantHttpClient _tenantHttpClient = TenantHttpClient;

    private HttpClient GetClient(bool useBaseClient)
    {
        return useBaseClient
            ? _baseHttpClient.GetPrivateHttpClient()
            : _tenantHttpClient.GetPrivateHttpClient();
    }

    public async Task<Result<T>> GetAllAsync<T>(string source, bool useBaseClient = false)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            var httpResponse = await client.GetAsync(new Uri(source, UriKind.RelativeOrAbsolute));

            if (httpResponse == null)
                return Result.Failure<T>(CustomError.Conflict("Connection Issue", "No response from server."));

            if (!httpResponse.IsSuccessStatusCode)
            {
                var problem = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

                if (problem is not null)
                    return Result.Failure<T>(CustomError.Failure(problem.Title!, problem.Detail!));

                return Result.Failure<T>(CustomError.Conflict("HTTP", $"Unexpected status: {httpResponse.StatusCode}"));

            }
            else
            {
                var result = await httpResponse.Content.ReadFromJsonAsync<Result<T>>();

                if (result is null)
                    return Result.Failure<T>(CustomError.Conflict("Deserialization", "Invalid response format."));

                return result.IsSuccess
                    ? Result.Success(result.Data)
                    : Result.Failure<T>(CustomError.Failure("API Error", "Request failed."));
            }
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict("Exception Occured", $"{ex.Message}"));
        }
    }

    public async Task<Result<T>> GetByIdAsync<T>(string basePath, string id, bool useBaseClient = false)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            var response = await client.GetFromJsonAsync<Result<T>>($"{basePath}/{id}");

            if (response == null)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            if (!response.IsSuccess)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            return Result.Success(response.Data);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }

    public async Task<Result> PostAsync<T>(string source, T obj, bool useBaseClient = true)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            var response = await client.PostAsJsonAsync(source, obj);

            if (response == null)
                return Result.Failure<T>(CustomError.Conflict("NETWORK", "No response from server."));

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Result>();

                if (result == null || result.IsFailure)
                    return Result.Failure<T>(CustomError.Conflict("RESPONSE", "Unexpected empty or failed result."));

                return Result.Success(result);
            }

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            if (problem != null)
            {
                return Result.Failure<T>(CustomError.Failure(problem.Title!, problem.Detail!));
            }

            return Result.Failure<T>(CustomError.Conflict("HTTP", $"Unexpected error: {response.StatusCode}"));
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }

    public async Task<Result> PutAsync<T>(string source, T obj)
    {
        try
        {
            HttpClient client = _baseHttpClient.GetPrivateHttpClient();
            var response = await client.PutAsJsonAsync(source, obj);

            var result = response.Content.ReadFromJsonAsync<Result>();

            if (response == null)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            if (result.IsFaulted)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            return response.IsSuccessStatusCode
                ? Result.Success()
                : Result.Failure(CustomError.Conflict(",", "No response from server."));
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }


    public async Task<Result> DeleteByIdAsync(string source, int id)
    {
        try
        {
            var deleteUri = new Uri($"{source}/{id}", UriKind.Relative);

            HttpClient client = GetClient(true);
            var response = await client.DeleteAsync(deleteUri);

            if (response == null)
                return Result.Failure(CustomError.Conflict(",", "No response from server."));

            return response.IsSuccessStatusCode ? Result.Success()
                : Result.Failure(CustomError.Conflict(",", "No response from server."));
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }
}
