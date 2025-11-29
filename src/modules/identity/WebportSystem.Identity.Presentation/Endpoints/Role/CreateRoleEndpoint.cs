namespace WebportSystem.Identity.Presentation.Endpoints.Role;

internal sealed class CreateRoleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("role", async (
            CreateRoleCommand request,
            ICommandHandler<CreateRoleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(request, cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Role);
    }
}