using System;
using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models;
using MVC.Models.AggregationStrategy;
using MVC.Models.ViewModels;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public partial class DataVisualizationController : BaseController
    {
        private readonly AggregationContext _aggregationContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<SortingType, Func<List<DataPoint>, decimal>> _sortingDictionary = [];

        public DataVisualizationController(AggregationContext aggregationContext, IServiceProvider serviceProvider)
        {
            _aggregationContext = aggregationContext;
            _serviceProvider = serviceProvider;

            _sortingDictionary.Add(SortingType.Sum, AggregationHelper.Sum);
            _sortingDictionary.Add(SortingType.Avg, AggregationHelper.Average);
            _sortingDictionary.Add(SortingType.Max, AggregationHelper.Max);
            _sortingDictionary.Add(SortingType.Min, AggregationHelper.Min);
        }

        // Shows consumption data
        [HttpGet("/consumption")]
        public async Task<IActionResult> Consumption(DataVisualizationViewModel viewModel)
        {
            try
            {
                BearerToken? bearerToken = GetBearerToken();

                // Strategy factory
                DataPointStrategyFactory factory = new DataPointStrategyFactory(_serviceProvider);

                // Creates the strategy based on the requested data type
                // Changes for different data types (consumption, price, cost)
                DataPointStrategy strategy = factory.Create(viewModel.RequestedDataType);

                // The strategy handles retrieving the correct data depending on the parameters
                List<DataPoint> dataPoints = await strategy.GetDataPoints(viewModel.SelectedDate,
                    viewModel.SelectedTimeframe,bearerToken);

                // Different timeframes handles the data differently
                _aggregationContext.SetStrategy(viewModel.SelectedTimeframe);

                // Aggregates the data using the strategy and looks up the sorting function in the dictionary
                AggregatedData aggregatedData = _aggregationContext.AggregateData(dataPoints,
                    _sortingDictionary[viewModel.SortingType]);

                DataVisualizationViewModel consumptionViewModel = new()
                {
                    AggregatedData = aggregatedData,
                    SelectedDate = viewModel.SelectedDate,
                    SelectedTimeframe = viewModel.SelectedTimeframe,
                    SortingType = viewModel.SortingType,
                    RequestedDataType = viewModel.RequestedDataType,
                };

                return View(consumptionViewModel);
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
