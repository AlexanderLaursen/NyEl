using System.Security.Claims;
using API.Controllers;
using API.Services;
using API.Services.Interfaces;
using Common.Dtos.Invoice;
using Common.Exceptions;
using Common.Models;
using Common.Models.TemplateGenerator;
using iText.Html2pdf;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/invoices")]
    public class InvoicesController : BaseController
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(
            ILogger<InvoicesController> logger,
            IInvoiceService invoiceService,
            IConsumerService consumerService)
            : base(consumerService)
        {
            _logger = logger;
            _invoiceService = invoiceService;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetInvoices()
        {
            try
            {
                int consumerId = await GetConsumerId();

                List<Invoice> invoices = await _invoiceService.GetInvoicesByIdAsync(consumerId);

                return Ok(invoices);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
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
                int consumerId = await GetConsumerId();

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
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching invoice.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadInvoicePdf(int id)
        {
            try
            {
                int consumerId = await GetConsumerId();

                Pdf pdf = await _invoiceService.GetPdfAsync(consumerId, id);

                return File(pdf.File, "application/pdf", "faktura.pdf");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting invoice.");
                return StatusCode(500);
            }
        }

    }
}