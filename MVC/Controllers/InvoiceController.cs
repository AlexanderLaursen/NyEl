using Common.Dtos.Consumer;
using Common.Dtos.Invoice;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models.ViewModels;
using MVC.Services;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IConsumerService _consumerService;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService invoiceService, IConsumerService consumerService, ILogger<InvoiceController> logger)
        {
            _invoiceService = invoiceService;
            _consumerService = consumerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");

                if (bearerToken == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                Result<List<Invoice>> result = await _invoiceService.GetInvoicesAsync(bearerToken);

                if (result == null)
                {
                    return View(new List<Invoice>());
                }

                return View(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching invoices.");
                return View(new List<Invoice>());
            }
        }

        [HttpGet("/invoice/details/{id}")]
        public async Task<IActionResult> Detailed(int id)
        {
            try
            {
                string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");
                if (bearerToken == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                Result<InvoiceDto> invoiceResult = await _invoiceService.GetInvoiceByIdAsync(id, bearerToken);
                Result<ConsumerDtoFull> consumerResult = await _consumerService.GetConsumerAsync(bearerToken);

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
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching invoice details.");
                return RedirectToAction("Index");
            }
        }
    }
}
