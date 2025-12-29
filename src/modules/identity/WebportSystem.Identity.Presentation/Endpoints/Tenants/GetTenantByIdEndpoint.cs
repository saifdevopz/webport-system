namespace WebportSystem.Identity.Presentation.Endpoints.Tenants;

internal sealed class GetTenantByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tenant/{Id}", async (
            int Id,
            IQueryHandler<GetTenantByIdQuery, GetTenantByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetTenantByIdQuery(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}
