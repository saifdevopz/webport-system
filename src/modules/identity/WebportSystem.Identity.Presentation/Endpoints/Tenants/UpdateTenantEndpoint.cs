namespace WebportSystem.Identity.Presentation.Endpoints.Tenants;

internal sealed class UpdateTenantEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("tenant", async (
            UpdateTenantCommand request,
            ICommandHandler<UpdateTenantCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(request, cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}
