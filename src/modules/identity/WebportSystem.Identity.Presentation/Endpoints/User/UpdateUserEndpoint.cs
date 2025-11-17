using WebportSystem.Identity.Presentation.Common;

namespace WebportSystem.Identity.Presentation.Endpoints.User;

internal sealed class UpdateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("user", async (
            UpdateUserCommand request,
            ICommandHandler<UpdateUserCommand> handler,
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