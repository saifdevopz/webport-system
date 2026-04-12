using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Inventory.Application.Features.BusinessProfile;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;

namespace WebportSystem.Inventory.Presentation.Endpoints;

internal sealed class BusinessProfileEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("BusinessProfile")
            .WithTags("Inventory.BusinessProfile")
            .RequireAuthorization();

        group.MapGet("", async (
            IQueryHandler<GetBusinessProfilesQuery, List<BusinessProfileDto>> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetBusinessProfilesQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            int id,
            IQueryHandler<GetBusinessProfileByIdQuery, BusinessProfileDto> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetBusinessProfileByIdQuery(id), cancellationToken)
                .MapResult();
        });

        group.MapPost("", async (
            CreateBusinessProfileCommand request,
            ICommandHandler<CreateBusinessProfileCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateBusinessProfileCommand request,
            ICommandHandler<UpdateBusinessProfileCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            int id,
            ICommandHandler<GenericDeleteCommand<BusinessProfileM>> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GenericDeleteCommand<BusinessProfileM>(id), cancellationToken)
                .MapResult();
        });
    }
}