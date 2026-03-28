namespace WebportSystem.Identity.Presentation.Endpoints;

internal sealed class TokenEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("token")
            .WithTags("Identity.Tokens")
            .AllowAnonymous();

        #region Commands

        group.MapPost("access", async (
            AccessTokenCommand request,
            ICommandHandler<AccessTokenCommand, AccessTokenResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });


        group.MapPost("refresh", async (
            RefreshTokenCommand request,
            ICommandHandler<RefreshTokenCommand, RefreshTokenResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        #endregion
    }
}