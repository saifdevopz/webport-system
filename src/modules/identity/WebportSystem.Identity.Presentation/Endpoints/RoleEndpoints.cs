namespace WebportSystem.Identity.Presentation.Endpoints;

internal sealed class RoleEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("role")
            .WithTags("Identity.Roles")
            .RequireAuthorization();

        #region Queries

        group.MapGet("", async (
            IQueryHandler<GetRolesQuery, GetRolesQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetRolesQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            string id,
            IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetRoleByIdQuery(id), cancellationToken)
                .MapResult();
        });

        #endregion

        #region Commands

        group.MapPost("", async (
            CreateRoleCommand request,
            ICommandHandler<CreateRoleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateRoleCommand request,
            ICommandHandler<UpdateRoleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            string id,
            ICommandHandler<DeleteRoleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new DeleteRoleCommand(id), cancellationToken)
                .MapResult();
        });

        #endregion
    }
}