using AdminPanel.Models.ViewModels;
using AdminPanel.Services;
using AdminPanel.Services.Interfaces;
using Common.Dtos.Invoice;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class AdminInvoiceController : MVC.Controllers.BaseController
    {
        private readonly IInvoiceService _invoiceService;

        public AdminInvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public IActionResult Index()
        {
            try
            {
                GetBearerToken();

                return View();
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

        // Generate single invoice including adding to the pdf queue. Requires timeframe and consumer id
        [HttpPost]
        public async Task<IActionResult> GenerateSingle(InvoiceViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index");
                }

                BearerToken? bearerToken = GetBearerToken();

                Timeframe timeframe = new Timeframe(viewModel.StartDate, viewModel.EndDate);

                Result<InvoiceDto> result = await _invoiceService.GenerateAsync(timeframe, viewModel.ConsumerId, bearerToken);

                if (!result.IsSuccess)
                {
                    return View("Index");
                }

                return View("Index");
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

        // Generates all invoices for the given timeframe including adding to the pdf queue
        [HttpPost]
        public async Task<IActionResult> GenerateAll(InvoiceViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index");
                }

                BearerToken? bearerToken = GetBearerToken();

                Timeframe timeframe = new Timeframe(viewModel.StartDate, viewModel.EndDate);

                Result<int> result = await _invoiceService.GenerateAllAsync(timeframe, bearerToken);

                if (!result.IsSuccess)
                {
                    return View("Index");
                }

                return View("Index");
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

        // Deletes single invoice
        [HttpPost]
        public async Task<IActionResult> Delete(InvoiceViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index");
                }

                BearerToken? bearerToken = GetBearerToken();

                Result<bool> result = await _invoiceService.DeleteInvoiceAsync(viewModel.InvoiceId, bearerToken);

                if (!result.IsSuccess)
                {
                    return View("Index");
                }

                return View("Index");
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
    }
}
