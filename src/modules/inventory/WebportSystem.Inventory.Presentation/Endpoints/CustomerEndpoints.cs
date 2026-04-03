using Microsoft.AspNetCore.Mvc;
using WebportSystem.Inventory.Application.Features.Customer;
using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Presentation.Endpoints;

internal sealed class CustomerEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("Customer")
            .WithTags("Inventory.Customers")
            .RequireAuthorization();

        #region Queries

        group.MapGet("", async (
            IQueryHandler<GetCustomersQuery, GetCustomersQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetCustomersQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            int id,
            IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetCustomerByIdQuery(id), cancellationToken)
                .MapResult();
        });

        #endregion

        #region Commands

        group.MapPost("", async (
            CreateCustomerCommand request,
            ICommandHandler<CreateCustomerCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateCustomerCommand request,
            ICommandHandler<UpdateCustomerCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            int id,
            [FromServices] ICommandHandler<GenericDeleteCommand<CustomerM>> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GenericDeleteCommand<CustomerM>(id), cancellationToken)
                .MapResult();
        });

        #endregion
    }
}