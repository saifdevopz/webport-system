namespace WebportSystem.Inventory.Presentation.Endpoints.Category;

internal sealed class UpdateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("category", async (
            UpdateCategoryCommand request,
            ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(request, cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Category)
        .RequireAuthorization();
    }
}