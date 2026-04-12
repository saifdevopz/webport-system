using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Inventory.Application.Features.Invoice;
using WebportSystem.Inventory.Domain.Entities.Invoice;

namespace WebportSystem.Inventory.Presentation.Endpoints;

internal sealed class InvoiceEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("Invoice")
            .WithTags("Inventory.Invoices")
            .RequireAuthorization();

        group.MapGet("", async (
            IQueryHandler<GetInvoicesQuery, List<InvoiceDto>> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetInvoicesQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            int id,
            IQueryHandler<GetInvoiceByIdQuery, InvoiceDto> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetInvoiceByIdQuery(id), cancellationToken)
                .MapResult();
        });

        group.MapPost("", async (
            CreateInvoiceCommand request,
            ICommandHandler<CreateInvoiceCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateInvoiceCommand request,
            ICommandHandler<UpdateInvoiceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            int id,
            ICommandHandler<GenericDeleteCommand<InvoiceM>> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GenericDeleteCommand<InvoiceM>(id), cancellationToken)
                .MapResult();
        });
    }
}