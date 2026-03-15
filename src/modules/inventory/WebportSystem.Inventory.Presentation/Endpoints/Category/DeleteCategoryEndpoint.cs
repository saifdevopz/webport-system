using Microsoft.AspNetCore.Mvc;
using WebportSystem.Inventory.Domain.Entities.Category;

namespace WebportSystem.Inventory.Presentation.Endpoints.Category;

internal sealed class DeleteCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("category/{Id}", async (
            int Id,
            [FromServices] ICommandHandler<GenericDeleteCommand<CategoryM>> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GenericDeleteCommand<CategoryM>(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Category)
        .RequireAuthorization();
    }
}