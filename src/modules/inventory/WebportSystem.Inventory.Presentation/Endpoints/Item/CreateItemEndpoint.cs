namespace WebportSystem.Inventory.Presentation.Endpoints.Item;

internal sealed class CreateItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("item", async (
            CreateItemCommand request,
            ICommandHandler<CreateItemCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(request, cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Item)
        .RequireAuthorization();
    }
}