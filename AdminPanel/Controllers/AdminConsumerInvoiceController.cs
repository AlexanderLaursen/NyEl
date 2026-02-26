using AdminPanel.Models.ViewModels;
using AdminPanel.Services;
using AdminPanel.Services.Interfaces;
using Common.Dtos.Consumer;
using Common.Dtos.Invoice;
using Common.Enums;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;

namespace AdminPanel.Controllers
{
    // Handles admin access to consumer invoices
    public class AdminConsumerInvoiceController : BaseController
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IConsumerService _consumerService;

        public AdminConsumerInvoiceController(IInvoiceService invoiceService, IConsumerService consumerService)
        {
            _invoiceService = invoiceService;
            _consumerService = consumerService;
        }

        public IActionResult Index(ConsumerInvoiceViewModel viewModel)
        {
            try
            {
                GetBearerToken();

                return View(viewModel);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Return all invoices by consumer id
        [HttpPost()]
        public async Task<IActionResult> GetInvoices(int consumerId)
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

                Result<List<Invoice>> result = await _invoiceService.GetInvoicesByConsumerIdAsync(consumerId, bearerToken);

                if (result == null || result.IsSuccess == false)
                {
                    return View(new List<Invoice>());
                }

                ConsumerInvoiceViewModel viewModel = new ConsumerInvoiceViewModel
                {
                    Invoices = result.Value,
                    ConsumerId = consumerId
                };

                return View("Index", viewModel);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Return single invoice by invoice id
        [HttpGet("/invoice/{id}")]
        public async Task<IActionResult> Detailed(int id)
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

                Result<InvoiceDto> invoiceResult = await _invoiceService.GetInvoiceByIdAsync(id, bearerToken);
                Result<ConsumerDtoFull> consumerResult = await _consumerService.GetConsumerAsync(invoiceResult.Value.ConsumerId, bearerToken);

                InvoiceDto invoiceDto = invoiceResult.Value;
                ConsumerDtoFull consumer = consumerResult.Value;

                if (invoiceResult == null)
                {
                    return RedirectToAction("Index");
                }

                DetailedInvoiceViewModel viewModel = new DetailedInvoiceViewModel
                {
                    InvoiceId = invoiceDto.Id,
                    BillingPeriodEnd = invoiceDto.BillingPeriodEnd,
                    BillingPeriodStart = invoiceDto.BillingPeriodStart,
                    TotalConsumption = invoiceDto.TotalConsumption,
                    TotalAmount = invoiceDto.TotalAmount,
                    PeriodData = invoiceDto.InvoicePeriodData.Adapt<List<InvoicePeriodData>>(),
                    ConsumerId = consumer.Id,
                    FirstName = consumer.FirstName,
                    LastName = consumer.LastName,
                    Address = consumer.Address,
                    City = consumer.City,
                    ZipCode = consumer.ZipCode.ToString(),
                    PhoneNumber = consumer.PhoneNumber,
                    Email = consumer.Email,
                    CPR = consumer.CPR.ToString(),
                    PaidStatus = invoiceDto.Paid ? "Ja" : "Nej",
                    BillingModelType = invoiceDto.BillingModel == null ? BillingModelType.FixedPrice : invoiceDto.BillingModel.BillingModelType
                };

                return View(viewModel);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // Download invoice as pdf
        [HttpPost()]
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

                Result<byte[]> result = await _invoiceService.GetPdfAsync(id, bearerToken);

                if (!result.IsSuccess || result == null || result.Value == null)
                {
                    return RedirectToAction("Detailed", new { id });
                }

                return File(result.Value, "application/pdf", $"invoice_{id}.pdf");
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
