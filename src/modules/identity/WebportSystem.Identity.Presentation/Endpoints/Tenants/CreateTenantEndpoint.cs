namespace WebportSystem.Identity.Presentation.Endpoints.Tenants;

internal sealed class CreateTenantEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("tenant", async (
            CreateTenantCommand request,
            ICommandHandler<CreateTenantCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(request, cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant)
        .RequireAuthorization();
    }
}
