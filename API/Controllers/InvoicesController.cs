using System.Security.Claims;
using API.Services.Interfaces;
using Common.Dtos.Invoice;
using Common.Exceptions;
using Common.Models;
using Common.Models.TemplateGenerator;
using iText.Html2pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly TemplateFactory _templateFactory;
    private readonly ILogger<InvoicesController> _logger;
    private readonly IInvoiceService _invoiceService;
    private readonly IConsumerService _consumerService;

    public InvoicesController(TemplateFactory templateFactory, ILogger<InvoicesController> logger, IInvoiceService invoiceService,
        IConsumerService consumerService)
    {
        _templateFactory = templateFactory;
        _logger = logger;
        _invoiceService = invoiceService;
        _consumerService = consumerService;
    }

    [Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetInvoices()
    {
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int consumerId = await _consumerService.GetConsumerId(userId);

            List<Invoice> invoices = await _invoiceService.GetInvoicesByIdAsync(consumerId);

            return Ok(invoices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching invoices.");
            return StatusCode(500);
        }
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetInvoice(int id)
    {
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int consumerId = await _consumerService.GetConsumerId(userId);

            Invoice invoice = await _invoiceService.GetInvoiceAsync(id);

            if (invoice.ConsumerId != consumerId)
            {
                return Unauthorized();
            }

            InvoiceDto invoiceDto = new()
            {
                Id = invoice.Id,
                BillingPeriodStart = invoice.BillingPeriodStart,
                BillingPeriodEnd = invoice.BillingPeriodEnd,
                TotalAmount = invoice.TotalAmount,
                TotalConsumption = invoice.TotalConsumption,
                Paid = invoice.Paid,
                ConsumerId = invoice.ConsumerId,
                BillingModel = invoice.BillingModel,
                InvoicePeriodData = invoice.InvoicePeriodData.Select(pd => new InvoicePeriodDto
                {
                    Consumption = pd.Consumption,
                    Cost = pd.Cost,
                    PeriodStart = pd.PeriodStart,
                    PeriodEnd = pd.PeriodEnd,
                    InvoiceId = pd.InvoiceId
                }).ToList()
            };

            return Ok(invoiceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching invoice.");
            return StatusCode(500);
        }
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateInvoice(Timeframe timeframe, int consumerId)
    {
        try
        {
            InvoiceDto invoiceDto = await _invoiceService.GenerateInvoice(timeframe, consumerId);

            return Ok(invoiceDto);
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

    [Authorize]
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadInvoicePdf(int id)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int consumerId = await _consumerService.GetConsumerId(userId);

        string htmlContent = await _invoiceService.CreatePdf(id, consumerId);

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