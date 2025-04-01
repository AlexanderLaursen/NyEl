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
    public class AdminInvoicesController : BaseController
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IInvoiceService _invoiceService;

        public AdminInvoicesController(
            ILogger<InvoicesController> logger,
            IInvoiceService invoiceService,
            IConsumerService consumerService)
            : base(consumerService)
        {
            _logger = logger;
            _invoiceService = invoiceService;
        }

        [HttpPost("generate")]
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

        [HttpPost("generate/all")]
        public async Task<IActionResult> GenerateAllInvoices()
        {
            try
            {
                // TODO ADD GENERATE ALL
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
    }
}