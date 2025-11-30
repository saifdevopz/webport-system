namespace WebportSystem.Identity.Presentation.Endpoints.Tenants;

internal sealed class DeleteTenantEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("tenant/{Id}", async (
            int Id,
            ICommandHandler<DeleteTenantCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new DeleteTenantCommand(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}