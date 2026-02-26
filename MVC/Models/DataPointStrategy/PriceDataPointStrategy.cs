using Common.Dtos.PriceInfo;
using Common.Enums;
using Common.Models;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public partial class DataVisualizationController
    {
        public class PriceDataPointStrategy : DataPointStrategy
        {
            private readonly IServiceProvider _serviceProvider;

            public PriceDataPointStrategy(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            // Gets price data
            public override async Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
            TimeframeOptions timeframeOptions, BearerToken? bearerToken)
            {
                // Prepares services from dependency injection
                IPriceInfoService priceInfoService = _serviceProvider.GetRequiredService<IPriceInfoService>();

                // Gets price information
                Result<PriceInfoListDto> result = await priceInfoService.GetPriceInfoAsync(
                        dateTime, timeframeOptions);

                if (!result.IsSuccess || result.Value == null)
                {
                    throw new Exception();
                }

                // Unwrap and map price information
                List<DataPoint> dataPoints = result.Value.PriceInfoList
                    .Select(cr => new DataPoint(cr.Timestamp, cr.PricePerKwh))
                    .ToList();

                List<DataPoint> sortedDatapoints = SortList(dataPoints);

                return sortedDatapoints;
            }

        }
    }
}
