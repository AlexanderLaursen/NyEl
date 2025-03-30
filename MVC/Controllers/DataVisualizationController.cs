using System;
using Common.Dtos.ConsumptionReading;
using Common.Dtos.PriceInfo;
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
    public class DataVisualizationController : BaseController
    {
        private readonly IConsumptionService _consumptionService;
        private readonly AggregationContext _aggregationContext;
        private readonly IPriceInfoService _priceInfoService;

        public DataVisualizationController(IConsumptionService consumptionService, AggregationContext aggregationContext,
            IPriceInfoService priceInfoService)
        {
            _consumptionService = consumptionService;
            _aggregationContext = aggregationContext;
            _priceInfoService = priceInfoService;
        }

        [HttpGet("/consumption")]
        public async Task<IActionResult> Consumption(DataVisualizationViewModel viewModel)
        {
            try
            {
                string bearerToken = GetBearerToken();

                Result<ConsumptionReadingListDto> result = await _consumptionService.GetConsumptionReadingsAsync(
                    viewModel.SelectedDate, viewModel.SelectedTimeframe, bearerToken!);

                if (!result.IsSuccess || result.Value == null)
                {
                    throw new Exception();
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
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        //public class DataPointStrategyFactory
        //{
        //    private readonly RequestedDataType _dataType;
        //    private readonly IServiceProvider _serviceProvider;

        //    public DataPointStrategyFactory(IServiceProvider serviceProvider, RequestedDataType dataType)
        //    {
        //        _serviceProvider = serviceProvider;
        //        _dataType = dataType;
        //    }

        //    public SuperDataPoint Create()
        //    {
        //        return _dataType switch
        //        {
        //            RequestedDataType.Consumption => new ConsumptionDataPointStrategy(_serviceProvider),
        //            RequestedDataType.Price => new PriceDataPointStrategy(_serviceProvider),
        //            RequestedDataType.Cost => new CostDataPointStrategy(_serviceProvider),
        //            _ => throw new ArgumentException()
        //        };
        //    }
        //}

        //public abstract class SuperDataPoint
        //{
        //    public abstract Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
        //        TimeframeOptions timeframeOptions, string? bearerToken = default);

        //    protected virtual List<DataPoint> SortList(List<DataPoint> dataPoints)
        //    {
        //        dataPoints.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

        //        return dataPoints;
        //    }

        //    protected virtual List<DataPoint> MatrixMultiplication(List<DataPoint> list1, List<DataPoint> list2)
        //    {
        //        List<DataPoint> dataPoints = new List<DataPoint>();

        //        if (list1.Count != list2.Count)
        //        {
        //            throw new Exception();
        //        }

        //        for (int i = 0; i < list1.Count;)
        //        {
        //            dataPoints.Add(new DataPoint(
        //                list1[i].Timestamp,
        //                list1[i].Value * list2[i].Value
        //                ));
        //        }

        //        return dataPoints;
        //    }
        //}

        //public class ConsumptionDataPointStrategy : SuperDataPoint
        //{
        //    private readonly IServiceProvider _serviceProvider;

        //    public ConsumptionDataPointStrategy(IServiceProvider serviceProvider)
        //    {
        //        _serviceProvider = serviceProvider;
        //    }

        //    public override async Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
        //    TimeframeOptions timeframeOptions, string? bearerToken = default)
        //    {
        //        IConsumptionService consumptionService = _serviceProvider.GetRequiredService<IConsumptionService>();

        //        Result<ConsumptionReadingListDto> result = await consumptionService.GetConsumptionReadingsAsync(
        //                dateTime, timeframeOptions, bearerToken!);

        //        if (!result.IsSuccess || result.Value == null)
        //        {
        //            throw new Exception();
        //        }

        //        List<DataPoint> dataPoints = result.Value.ConsumptionReadings
        //            .Select(cr => new DataPoint(cr.Timestamp, cr.Consumption))
        //            .ToList();

        //        List<DataPoint> sortedDatapoints = SortList(dataPoints);

        //        return sortedDatapoints;
        //    }
        //}

        //public class PriceDataPointStrategy : SuperDataPoint
        //{
        //    private readonly IServiceProvider _serviceProvider;

        //    public PriceDataPointStrategy(IServiceProvider serviceProvider)
        //    {
        //        _serviceProvider = serviceProvider;
        //    }

        //    public override async Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
        //    TimeframeOptions timeframeOptions, string? bearerToken = default)
        //    {
        //        IPriceInfoService priceInfoService = _serviceProvider.GetRequiredService<IPriceInfoService>();

        //        Result<PriceInfoListDto> result = await priceInfoService.GetPriceInfoAsync(
        //                dateTime, timeframeOptions);

        //        if (!result.IsSuccess || result.Value == null)
        //        {
        //            throw new Exception();
        //        }

        //        List<DataPoint> dataPoints = result.Value.PriceInfoList
        //            .Select(cr => new DataPoint(cr.Timestamp, cr.PricePerKwh))
        //            .ToList();

        //        List<DataPoint> sortedDatapoints = SortList(dataPoints);

        //        return sortedDatapoints;
        //    }

        //}

        //public class CostDataPointStrategy : SuperDataPoint
        //{
        //    private readonly IServiceProvider _serviceProvider;

        //    public CostDataPointStrategy(IServiceProvider serviceProvider)
        //    {
        //        _serviceProvider = serviceProvider;
        //    }

        //    public override async Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
        //        TimeframeOptions timeframeOptions, string? bearerToken = default)
        //    {
        //        IConsumptionService consumptionService = _serviceProvider.GetRequiredService<IConsumptionService>();
        //        IPriceInfoService priceInfoService = _serviceProvider.GetRequiredService<IPriceInfoService>();

        //        Result<ConsumptionReadingListDto> consumptionResult = await consumptionService.GetConsumptionReadingsAsync(
        //                dateTime, timeframeOptions, bearerToken!);
        //        Result<PriceInfoListDto> priceResult = await priceInfoService.GetPriceInfoAsync(
        //                dateTime, timeframeOptions);

        //        if (!consumptionResult.IsSuccess || consumptionResult.Value == null
        //            || !priceResult.IsSuccess || priceResult.Value == null)
        //        {
        //            throw new Exception();
        //        }

        //        List<DataPoint> consumptionDataPoints = consumptionResult.Value.ConsumptionReadings
        //            .Select(cr => new DataPoint(cr.Timestamp, cr.Consumption))
        //            .ToList();

        //        List<DataPoint> priceDataPoints = priceResult.Value.PriceInfoList
        //             .Select(cr => new DataPoint(cr.Timestamp, cr.PricePerKwh))
        //             .ToList();

        //        List<DataPoint> dataPoints = MatrixMultiplication(priceDataPoints, consumptionDataPoints);
        //        List<DataPoint> sortedDatapoints = SortList(dataPoints);

        //        return dataPoints;
        //    }
        //}
    }
}
