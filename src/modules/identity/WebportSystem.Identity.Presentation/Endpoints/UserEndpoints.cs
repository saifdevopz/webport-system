namespace WebportSystem.Identity.Presentation.Endpoints;

internal sealed class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("user")
            .WithTags("Identity.Users");

        #region Queries

        group.MapGet("", async (
            IQueryHandler<GetUsersQuery, GetUsersQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetUsersQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            string id,
            IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetUserByIdQuery(id), cancellationToken)
                .MapResult();
        });

        #endregion

        #region Commands

        group.MapPost("", async (
            CreateUserCommand request,
            ICommandHandler<CreateUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateUserCommand request,
            ICommandHandler<UpdateUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            int id,
            ICommandHandler<DeleteUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new DeleteUserCommand(id), cancellationToken)
                .MapResult();
        });

        #endregion
    }
}