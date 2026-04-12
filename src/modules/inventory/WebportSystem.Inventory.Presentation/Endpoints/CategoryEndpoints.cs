using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Inventory.Domain.Entities.Category;
using static WebportSystem.Inventory.Application.Features.Category.CreateCategoryCommandValidator;

namespace WebportSystem.Inventory.Presentation.Endpoints;

internal sealed class CategoryEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("Category")
            .WithTags("Inventory.Categories")
            .RequireAuthorization();

        group.MapGet("", async (
            IQueryHandler<GetCategoriesQuery, List<CategoryDto>> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetCategoriesQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            int id,
            IQueryHandler<GetCategoryByIdQuery, CategoryDto> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetCategoryByIdQuery(id), cancellationToken)
                .MapResult();
        });

        group.MapPost("", async (
            CreateCategoryCommand request,
            ICommandHandler<CreateCategoryCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateCategoryCommand request,
            ICommandHandler<UpdateCategoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            int id,
            ICommandHandler<GenericDeleteCommand<CategoryM>> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GenericDeleteCommand<CategoryM>(id), cancellationToken)
                .MapResult();
        });
    }
}