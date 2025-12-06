namespace WebportSystem.Inventory.Presentation.Endpoints.Item;

internal sealed class UpdateItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("item", async (
            UpdateItemCommand request,
            ICommandHandler<UpdateItemCommand> handler,
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