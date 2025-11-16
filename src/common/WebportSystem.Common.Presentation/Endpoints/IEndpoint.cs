using Microsoft.AspNetCore.Routing;

namespace WebportSystem.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}