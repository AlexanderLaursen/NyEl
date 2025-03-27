using API.Services.Interfaces;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Common.Models.TemplateGenerator;
using iText.Html2pdf;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly TemplateFactory _templateFactory;
    private readonly ILogger<InvoiceController> _logger;
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(TemplateFactory templateFactory, ILogger<InvoiceController> logger, IInvoiceService invoiceService)
    {
        _templateFactory = templateFactory;
        _logger = logger;
        _invoiceService = invoiceService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateInvoice(Timeframe timeframe, int consumerId)
    {
        try
        {
            Invoice invoice = await _invoiceService.GenerateInvoice(timeframe, consumerId);

            return Ok(invoice);
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

    [HttpPost("generate/all")]
    public async Task<IActionResult> GenerateAllInvoices()
    {
        try
        {

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

    [HttpGet("download")]
    public IActionResult DownloadInvoicePdf()
    {
        var templateGenerator = _templateFactory.CreateTemplateGenerator(TemplateType.Invoice);
        string htmlContent = templateGenerator.GenerateTemplate();

        byte[] pdfBytes;
        using (var memoryStream = new MemoryStream())
        {
            HtmlConverter.ConvertToPdf(htmlContent, memoryStream);
            pdfBytes = memoryStream.ToArray();
        }

        return File(pdfBytes, "application/pdf", "invoice.pdf");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        try
        {
            bool isDeleted = await _invoiceService.DeleteInvoice(id);
            if (isDeleted)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting invoice.");
            return StatusCode(500);
        }
    }
}