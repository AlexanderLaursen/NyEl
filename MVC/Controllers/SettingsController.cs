using Common.Dtos.BillingModel;
using Common.Dtos.Consumer;
using Common.Dtos.InvoicePreference;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models.ViewModels;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("/settings")]
        public IActionResult Index()
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

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
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost("/settings/update")]
        public async Task<IActionResult> UpdateSettings(SettingsViewModel viewModel)
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

                InvoicePreferenceListDto invoicePreferenceListDto = new()
                {
                    InvoicePreferences = viewModel.InvoicePreferences
                };

                BillingModelDto billingModelDto = new()
                {
                    BillingModelType = viewModel.BillingModel
                };

                Result<bool> result = await _settingsService.UpdateSettingsAsync(invoicePreferenceListDto, billingModelDto, bearerToken);

                return RedirectToAction("Index");
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}
