using WebportSystem.Inventory.Application.Features.BusinessProfile;

namespace WebportSystem.Inventory.Presentation.Endpoints;

internal sealed class BusinessProfileEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("BusinessProfile")
            .WithTags("Inventory.BusinessProfile")
            .RequireAuthorization();

        #region Queries

        group.MapGet("", async (
            IQueryHandler<GetBusinessProfilesQuery, GetBusinessProfilesQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetBusinessProfilesQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            int id,
            IQueryHandler<GetBusinessProfileByIdQuery, GetBusinessProfileByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetBusinessProfileByIdQuery(id), cancellationToken)
                .MapResult();
        });

        #endregion

        #region Commands

        group.MapPost("", async (
            CreateBusinessProfileCommand request,
            ICommandHandler<CreateBusinessProfileCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateBusinessProfileCommand request,
            ICommandHandler<UpdateBusinessProfileCommand, UpdateBusinessProfileResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        #endregion
    }
}