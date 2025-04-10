using Common.Dtos.ConsumptionReading;
using Common.Dtos.PriceInfo;
using Common.Enums;
using Common.Models;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public partial class DataVisualizationController
    {
        public class CostDataPointStrategy : DataPointStrategy
        {
            private readonly IServiceProvider _serviceProvider;

            public CostDataPointStrategy(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public override async Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
                TimeframeOptions timeframeOptions, BearerToken? bearerToken)
            {
                IConsumptionService consumptionService = _serviceProvider.GetRequiredService<IConsumptionService>();
                IPriceInfoService priceInfoService = _serviceProvider.GetRequiredService<IPriceInfoService>();

                Result<ConsumptionReadingListDto> consumptionResult = await consumptionService.GetConsumptionReadingsAsync(
                        dateTime, timeframeOptions, bearerToken);
                Result<PriceInfoListDto> priceResult = await priceInfoService.GetPriceInfoAsync(
                        dateTime, timeframeOptions);

                if (!consumptionResult.IsSuccess || consumptionResult.Value == null
                    || !priceResult.IsSuccess || priceResult.Value == null)
                {
                    throw new Exception();
                }

                List<DataPoint> consumptionDataPoints = consumptionResult.Value.ConsumptionReadings
                    .Select(cr => new DataPoint(cr.Timestamp, cr.Consumption))
                    .ToList();

                List<DataPoint> priceDataPoints = priceResult.Value.PriceInfoList
                     .Select(cr => new DataPoint(cr.Timestamp, cr.PricePerKwh))
                     .ToList();

                List<DataPoint> dataPoints = NestedMultiplication(priceDataPoints, consumptionDataPoints);
                List<DataPoint> sortedDatapoints = SortList(dataPoints);

                return dataPoints;
            }
        }
    }
}
