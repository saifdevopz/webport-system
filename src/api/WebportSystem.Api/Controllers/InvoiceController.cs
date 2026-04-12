using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Infrastructure.QuestPDF;
using WebportSystem.Inventory.Application.Features.Invoice;

namespace WebportSystem.Api.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class InvoiceController(
    IQueryHandler<GetInvoicePrintQuery, InvoicePrintDto> handler) : ControllerBase
{
    [HttpGet("print")]
    public async Task<ActionResult> GetItemsInvoice(int InvoiceId, CancellationToken cancellationToken)
    {
        var invoice = await handler.Handle(new GetInvoicePrintQuery(InvoiceId), cancellationToken);

        if (invoice == null)
        {
            return NotFound();
        }

        var document = new InvoiceItemDocument(invoice.Data);

        var pdf = document.GeneratePdf();
        return File(pdf, "application/pdf", $"invoice-{InvoiceId}.pdf");
    }
}