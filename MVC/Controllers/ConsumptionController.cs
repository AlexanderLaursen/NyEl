using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models.ViewModels;
using MVC.Services.Interfaces;
using ConsumptionReading = MVC.Models.ConsumptionReading;

namespace MVC.Controllers
{
    public class ConsumptionController : Controller
    {
        private readonly IConsumptionService _consumptionService;

        public ConsumptionController(IConsumptionService consumptionService)
        {
            _consumptionService = consumptionService;
        }

        [HttpGet("/consumption")]
        public async Task<IActionResult> Index()
        {
            DateTime testDate = new DateTime(2025, 03, 23);

            string? userId = HttpContext.Session.GetJson<string>("Bearer");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Result<ConsumptionReadingListDto> result = await _consumptionService.GetConsumptionReadingsAsync(testDate, TimeframeOptions.Daily, userId);

            if (!result.IsSuccess || result.Value == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ConsumptionViewModel consumptionViewModel = new()
            {
                ConsumptionReadings = result.Value.ConsumptionReadings
                    .Select(dto => new ConsumptionReading
                    {
                        Timestamp = dto.Timestamp.ToString("HH:mm"),
                        Consumption = dto.Consumption
                    })
                    .ToList()
            };

            return View(consumptionViewModel);
        }
    }
}
