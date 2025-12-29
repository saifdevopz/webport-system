namespace WebportSystem.Api.Extensions;

internal static class MiddlewareExtensions
{
    internal static IApplicationBuilder UseApplicationMiddlewares(this IApplicationBuilder app)
    {
        return app;
    }
}