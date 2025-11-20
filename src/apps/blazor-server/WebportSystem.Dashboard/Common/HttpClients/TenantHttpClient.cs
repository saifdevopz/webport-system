using System.Net.Http.Headers;

namespace WebportSystem.Dashboard.Common.HttpClients;

public sealed class TenantHttpClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
{
    private const string HeaderKey = "Authorization";
    public HttpClient GetPrivateHttpClient()
    {
        HttpClient client = httpClientFactory.CreateClient(nameof(TenantHttpClient));

        string? stringToken = httpContextAccessor?.HttpContext?.Request.Cookies[BlazorConstants.AuthCookieName];
        if (string.IsNullOrEmpty(stringToken))
        {
            return client;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", stringToken);
        return client;
    }

    public HttpClient GetPublicHttpClient()
    {
        HttpClient client = httpClientFactory.CreateClient(nameof(TenantHttpClient));
        client.DefaultRequestHeaders.Remove(HeaderKey);
        return client;
    }
}