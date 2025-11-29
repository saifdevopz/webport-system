namespace WebportSystem.Identity.Presentation.Endpoints.Role;

internal sealed class UpdateRoleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("role", async (
            UpdateRoleCommand request,
            ICommandHandler<UpdateRoleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(request, cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Role)
        .RequireAuthorization();
    }
}