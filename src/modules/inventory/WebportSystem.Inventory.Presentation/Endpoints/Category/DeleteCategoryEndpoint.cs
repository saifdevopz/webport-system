namespace WebportSystem.Inventory.Presentation.Endpoints.Category;

internal sealed class DeleteCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("category/{Id}", async (
            int Id,
            ICommandHandler<DeleteCategoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new DeleteCategoryCommand(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Category)
        .RequireAuthorization();
    }
}