using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models;
using MVC.Models.AggregationStrategy;
using MVC.Models.ViewModels;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class DataVisualizationController : Controller
    {
        private readonly IConsumptionService _consumptionService;
        private readonly AggregationContext _aggregationContext;

        public DataVisualizationController(IConsumptionService consumptionService, AggregationContext aggregationContext)
        {
            _consumptionService = consumptionService;
            _aggregationContext = aggregationContext;
        }

        [HttpGet("/consumption")]
        public async Task<IActionResult> Consumption(DataVisualizationViewModel viewModel)
        {
            string? userId = HttpContext.Session.GetJson<string>("Bearer");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (viewModel.SelectedDate == DateTime.MinValue)
            {
                viewModel.SelectedDate = new DateTime(2025, 03, 23);
            }

            if (viewModel.SelectedTimeframe == TimeframeOptions.None)
            {
                viewModel.SelectedTimeframe = TimeframeOptions.Daily;
            }

            Result<ConsumptionReadingListDto> result = await _consumptionService.GetConsumptionReadingsAsync(viewModel.SelectedDate, viewModel.SelectedTimeframe, userId);

            if (!result.IsSuccess || result.Value == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<DataPoint> dataPoints = result.Value.ConsumptionReadings
                .Select(cr => new DataPoint(cr.Timestamp, cr.Consumption))
                .ToList();

            _aggregationContext.SetStrategy(viewModel.SelectedTimeframe);
            AggregatedData aggregatedData = _aggregationContext.AggregateData(dataPoints, AggregationHelper.Sum);

            DataVisualizationViewModel consumptionViewModel = new()
            {
                AggregatedData = aggregatedData,
                SelectedDate = viewModel.SelectedDate,
                SelectedTimeframe = viewModel.SelectedTimeframe
            };

            return View(consumptionViewModel);
        }
    }
}
