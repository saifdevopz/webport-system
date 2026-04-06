using WebportSystem.Inventory.Application.Features.Invoice;

namespace WebportSystem.Inventory.Presentation.Endpoints;

internal sealed class InvoiceEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("Invoice")
            .WithTags("Inventory.Invoices")
            .RequireAuthorization();

        group.MapGet("", async (
            IQueryHandler<GetInvoicesQuery, GetInvoicesQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetInvoicesQuery(), cancellationToken)
                .MapResult();
        });

        group.MapGet("{id}", async (
            int id,
            IQueryHandler<GetInvoiceByIdQuery, GetInvoiceByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new GetInvoiceByIdQuery(id), cancellationToken)
                .MapResult();
        });

        group.MapPost("", async (
            CreateInvoiceCommand request,
            ICommandHandler<CreateInvoiceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapPut("", async (
            UpdateInvoiceCommand request,
            ICommandHandler<UpdateInvoiceCommand, UpdateInvoiceResult> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(request, cancellationToken)
                .MapResult();
        });

        group.MapDelete("{id}", async (
            int id,
            ICommandHandler<DeleteInvoiceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            return await handler
                .Handle(new DeleteInvoiceCommand(id), cancellationToken)
                .MapResult();
        });
    }
}