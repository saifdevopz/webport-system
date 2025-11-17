using WebportSystem.Identity.Presentation.Common;

namespace WebportSystem.Identity.Presentation.Endpoints.User;

internal sealed class GetUsersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("user", async (
            IQueryHandler<GetUsersQuery, GetUsersQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetUsersQuery(), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}
