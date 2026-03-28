namespace WebportSystem.Inventory.Presentation.Endpoints;

internal sealed class CategoryEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("Category")
            .WithTags("Inventory.Categories")
            .RequireAuthorization();

        #region Queries

        group.MapGet("", async (
            IQueryHandler<GetCategoriesQuery, GetCategoriesQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetCategoriesQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            int id,
            IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetCategoryByIdQuery(id), cancellationToken)
                .MapResult();
        });

        #endregion

        #region Commands

        group.MapPost("", async (
            CreateCategoryCommand request,
            ICommandHandler<CreateCategoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateCategoryCommand request,
            ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            int id,
            ICommandHandler<DeleteCategoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new DeleteCategoryCommand(id), cancellationToken)
                .MapResult();
        });

        #endregion
    }
}