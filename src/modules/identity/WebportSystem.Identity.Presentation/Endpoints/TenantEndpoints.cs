namespace WebportSystem.Identity.Presentation.Endpoints;

internal sealed class TenantEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("Tenant")
            .WithTags("Identity.Tenants")
            .RequireAuthorization();

        #region Queries

        group.MapGet("", async (
            IQueryHandler<GetTenantsQuery, GetTenantsQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetTenantsQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            int id,
            IQueryHandler<GetTenantByIdQuery, GetTenantByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetTenantByIdQuery(id), cancellationToken)
                .MapResult();
        });

        #endregion

        #region Commands

        group.MapPost("", async (
            CreateTenantCommand request,
            ICommandHandler<CreateTenantCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateTenantCommand request,
            ICommandHandler<UpdateTenantCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            int id,
            ICommandHandler<DeleteTenantCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new DeleteTenantCommand(id), cancellationToken)
                .MapResult();
        });

        #endregion
    }
}
