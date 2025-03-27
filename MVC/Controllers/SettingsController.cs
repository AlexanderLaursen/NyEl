using Common.Dtos.Consumer;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using MVC.Helpers;
using MVC.Models.ViewModels;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("/settings")]
        public IActionResult Index()
        {
            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");

            if (bearerToken == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Result<ConsumerDtoFull> result = _settingsService.GetSettingsAsync(bearerToken).Result;

            if (!result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            SettingsViewModel settingsViewModel = new()
            {
                FirstName = result.Value.FirstName,
                LastName = result.Value.LastName,
                PhoneNumber = result.Value.PhoneNumber,
                Email = result.Value.Email,
                CPR = result.Value.CPR,
                BillingModel = result.Value.BillingModel,
                InvoicePreferences = result.Value.InvoicePreferences
            };

            return View(settingsViewModel);
        }

        // TODO Implement
        [HttpPost("/settings/update")]
        public IActionResult UpdateSettings(SettingsViewModel viewModel)
        {
            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");

            if (bearerToken == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
    }
}
