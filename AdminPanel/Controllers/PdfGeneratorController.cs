using Common.Enums;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using AdminPanel.Services.Interfaces;
using Common.Models;
using AdminPanel.Models;
using AdminPanel.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Common.Exceptions;

namespace AdminPanel.Controllers
{
    public class PdfGeneratorController : MVC.Controllers.BaseController
    {
        private readonly IPdfService _pdfService;
        public PdfGeneratorController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        // Retrieves full status from the pdf generator service running on the API
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

                Result<PdfFullStatus> result = await _pdfService.GetFullStatusAsync(bearerToken);

                PdfStatusViewModel viewModel = new PdfStatusViewModel
                {
                    GUID = result.Value.GUID,
                    Status = result.Value.Status,
                    QueueLength = result.Value.QueueLength,
                    DelayActive = result.Value.DelayActive,
                    Delay = result.Value.Delay,
                    QueueCheckInterval = result.Value.QueueCheckInterval
                };

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


        [HttpPost]
        public async Task<IActionResult> SetTestDelay(int delay)
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

                await _pdfService.SetTestDelay(delay, bearerToken);

                return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<IActionResult> SetDelayActive(bool delayActive)
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

                await _pdfService.SetDelayActive(delayActive, bearerToken);

                return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<IActionResult> SetQueueCheckInterval(int queueInterval)
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

                await _pdfService.SetQueueCheckInterval(queueInterval, bearerToken);

                return RedirectToAction("Index");
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
