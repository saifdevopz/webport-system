namespace WebportSystem.Inventory.Presentation.Endpoints.Category;

internal sealed class GetCategoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("category", async (
            IQueryHandler<GetCategoriesQuery, GetCategoriesQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetCategoriesQuery(), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Category)
        .RequireAuthorization();
    }
}