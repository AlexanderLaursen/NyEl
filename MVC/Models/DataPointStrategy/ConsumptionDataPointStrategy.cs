using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Models;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public partial class DataVisualizationController
    {
        public class ConsumptionDataPointStrategy : DataPointStrategy
        {
            private readonly IServiceProvider _serviceProvider;

            public ConsumptionDataPointStrategy(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            // Retrieves consumption data
            public override async Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
            TimeframeOptions timeframeOptions, BearerToken? bearerToken)
            {
                // Get consumptionService from dependency injection
                IConsumptionService consumptionService = _serviceProvider.GetRequiredService<IConsumptionService>();

                // Uses consumptionService to get consumption readings from database
                Result<ConsumptionReadingListDto> result = await consumptionService.GetConsumptionReadingsAsync(
                        dateTime, timeframeOptions, bearerToken);

                if (!result.IsSuccess || result.Value == null)
                {
                    throw new Exception();
                }

                // Unwraps result and maps to DataPoint
                List<DataPoint> dataPoints = result.Value.ConsumptionReadings
                    .Select(cr => new DataPoint(cr.Timestamp, cr.Consumption))
                    .ToList();

                List<DataPoint> sortedDatapoints = SortList(dataPoints);

                return sortedDatapoints;
            }
        }
    }
}
