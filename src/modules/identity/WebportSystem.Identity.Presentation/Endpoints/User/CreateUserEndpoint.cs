using WebportSystem.Identity.Presentation.Common;

namespace WebportSystem.Identity.Presentation.Endpoints.User;

internal sealed class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user", async (
            CreateUserCommand request,
            ICommandHandler<CreateUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(request, cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}
