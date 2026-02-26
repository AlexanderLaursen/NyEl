using System.Security.Claims;
using API.Controllers;
using API.Services;
using API.Services.Interfaces;
using Common.Dtos.Invoice;
using Common.Exceptions;
using Common.Models;
using iText.Html2pdf;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/invoices")]
    public class AdminInvoicesController : BaseController
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IInvoiceService _invoiceService;
        private readonly IConsumerService _consumerService;

        public AdminInvoicesController(
            ILogger<InvoicesController> logger,
            IInvoiceService invoiceService,
            IConsumerService consumerService)
            : base(consumerService)
        {
            _logger = logger;
            _invoiceService = invoiceService;
            _consumerService = consumerService;
        }

        [Authorize(Roles = "Admins")]
        [HttpPost("generate/{consumerId}")]
        public async Task<IActionResult> GenerateInvoice(Timeframe timeframe, int consumerId)
        {
            try
            {
                Invoice invoice = await _invoiceService.GenerateInvoice(timeframe, consumerId);
                InvoiceDto invoiceDto = invoice.Adapt<InvoiceDto>();

                return Ok(invoiceDto);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
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


        [Authorize(Roles = "Admins")]
        [HttpPost("generate/all")]
        public async Task<IActionResult> GenerateAllInvoices(Timeframe timeframe)
        {
            try
            {
                List<int> consumerIds = await _consumerService.GetAllActiveConsumerIds();

                foreach (int consumerId in consumerIds)
                {
                    await _invoiceService.GenerateInvoice(timeframe, consumerId);
                }

                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
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

        [Authorize(Roles = "Admins")]
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

        [Authorize(Roles = "Admins")]
        [HttpGet("consumer/{id:int}")]
        public async Task<IActionResult> GetInvoices(int id)
        {
            try
            {
                List<Invoice> invoices = await _invoiceService.GetInvoicesByIdAsync(id);

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

        [Authorize(Roles = "Admins")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            try
            {
                Invoice invoice = await _invoiceService.GetInvoiceAsync(id);

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

        [Authorize(Roles = "Admins")]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadInvoicePdf(int id)
        {
            try
            {
                Pdf pdf = await _invoiceService.GetPdfAdminAsync(id);

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