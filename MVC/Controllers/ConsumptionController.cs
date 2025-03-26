using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models.AggregationStrategy;
using MVC.Models.ViewModels;
using MVC.Services.Interfaces;
using ConsumptionReading = MVC.Models.ConsumptionReading;

namespace MVC.Controllers
{
    public class ConsumptionController : Controller
    {
        private readonly IConsumptionService _consumptionService;
        private readonly AggregationContext _aggregationContext;

        public ConsumptionController(IConsumptionService consumptionService, AggregationContext aggregationContext)
        {
            _consumptionService = consumptionService;
            _aggregationContext = aggregationContext;
        }

        [HttpGet("/consumption")]
        public async Task<IActionResult> Index(ConsumptionViewModel viewModel)
        {
            string? userId = HttpContext.Session.GetJson<string>("Bearer");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Console.WriteLine(viewModel.SelectedDate);
            Console.WriteLine(viewModel.SelectedTimeframe);
            if (viewModel.SelectedDate == DateTime.MinValue)
            {
                viewModel.SelectedDate = new DateTime(2025, 03, 23);
            }

            if (viewModel.SelectedTimeframe == TimeframeOptions.None)
            {
                viewModel.SelectedTimeframe = TimeframeOptions.Daily;
            }

            Result<ConsumptionReadingListDto> result = await _consumptionService.GetConsumptionReadingsAsync(viewModel.SelectedDate, TimeframeOptions.Daily, userId);

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
                    .ToList(),
                SelectedDate = viewModel.SelectedDate,
                SelectedTimeframe = viewModel.SelectedTimeframe
            };

            return View(consumptionViewModel);
        }
    }
}
