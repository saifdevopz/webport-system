namespace WebportSystem.Inventory.Presentation.Endpoints.Item;

internal sealed class GetItemByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("item/{Id}", async (
            int Id,
            IQueryHandler<GetItemByIdQuery, GetItemByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetItemByIdQuery(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Item)
        .RequireAuthorization();
    }
}
