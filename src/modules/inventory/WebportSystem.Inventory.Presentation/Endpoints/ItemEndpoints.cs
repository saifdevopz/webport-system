namespace WebportSystem.Inventory.Presentation.Endpoints;

internal sealed class ItemEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("Item")
            .WithTags("Inventory.Items")
            .RequireAuthorization();

        group.MapGet("", async (
            IQueryHandler<GetItemsQuery, GetItemsQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetItemsQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            int id,
            IQueryHandler<GetItemByIdQuery, GetItemByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetItemByIdQuery(id), cancellationToken)
                .MapResult();
        });

        group.MapPost("", async (
            CreateItemCommand request,
            ICommandHandler<CreateItemCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateItemCommand request,
            ICommandHandler<UpdateItemCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            int id,
            ICommandHandler<DeleteItemCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new DeleteItemCommand(id), cancellationToken)
                .MapResult();
        });
    }
}