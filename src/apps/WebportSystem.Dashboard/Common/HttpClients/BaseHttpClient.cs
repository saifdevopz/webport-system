using System.Net.Http.Headers;

namespace WebportSystem.Dashboard.Common.HttpClients;

public sealed class BaseHttpClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
{
    private const string HeaderKey = "Authorization";
    public HttpClient GetPrivateHttpClient()
    {
        HttpClient client = httpClientFactory.CreateClient(nameof(BaseHttpClient));

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return client;
        }

        if (!httpContext.Request.Cookies.TryGetValue(BlazorConstants.AuthCookieName, out var stringToken)
                || string.IsNullOrEmpty(stringToken))
        {
            return client;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", stringToken);
        return client;
    }

    public HttpClient GetPublicHttpClient()
    {
        HttpClient client = httpClientFactory.CreateClient(nameof(BaseHttpClient));

        if (client.DefaultRequestHeaders.Contains(HeaderKey))
        {
            client.DefaultRequestHeaders.Remove(HeaderKey);
        }

        return client;
    }
}
