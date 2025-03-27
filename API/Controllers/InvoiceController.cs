using System.Security.Claims;
using API.Data;
using API.Services.Interfaces;
using Common.Enums;
using Common.Exceptions;
using Common.Models.TemplateGenerator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly DataContext _context;
    private readonly TemplateFactory _templateFactory;
    private readonly ILogger<InvoiceController> _logger;
    private readonly IConsumerService _consumerService;

    public InvoiceController(DataContext context, TemplateFactory templateFactory, ILogger<InvoiceController> logger,
        IConsumerService consumerService)
    {
        _context = context;
        _templateFactory = templateFactory;
        _logger = logger;
        _consumerService = consumerService;
    }

    [Authorize]
    [HttpGet("generate")]
    public async Task<IActionResult> GenerateInvoicePdf(int invoiceId)
    {
        try
        {
            //string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //int consumerId = await _consumerService.GetConsumerId(userId);

            //var invoice = await _context.Invoices.FindAsync(invoiceId);

            //if (invoice == null)
            //{
            //    return NotFound();
            //}

            //var templateGenerator = _templateFactory.CreateTemplateGenerator(templateType);
            //string htmlContent = templateGenerator.GenerateInvoiceHtml(invoice);

            //using (var pdfStream = new MemoryStream())
            //{
            //    HtmlToPdf converter = new HtmlToPdf();
            //    PdfDocument doc = converter.ConvertHtmlString(htmlContent);
            //    doc.Save(pdfStream);
            //    pdfStream.Position = 0;
            //    doc.Close();

            //    return File(pdfStream.ToArray(), "application/pdf", $"Invoice_{invoice.Id}.pdf");
            //}
            return Ok();
        }
        catch (UnkownUserException ex)
        {
            _logger.LogWarning(ex, $"User not found.");
            return Unauthorized();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating invoice PDF.");
            return StatusCode(500);
        }
    }
}