using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Infrastructure.QuestPDF;
using WebportSystem.Inventory.Application.Features.Invoice;

namespace WebportSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class InvoiceController(
    IQueryHandler<GetInvoicePrintQuery, GetInvoicePrintQueryResult> handler) : ControllerBase
{
    [HttpGet("pdf4")]
    public async Task<ActionResult> GetInvoice4(int InvoiceId, CancellationToken cancellationToken)
    {
        var invoice = await handler.Handle(new GetInvoicePrintQuery(InvoiceId), cancellationToken);

        if (invoice == null)
        {
            return NotFound();
        }

        var document = new InvoiceItemDocument(invoice.Data.Records);
        var pdf = document.GeneratePdf();

        return File(pdf, "application/pdf", "webport-invoice.pdf");
    }
}