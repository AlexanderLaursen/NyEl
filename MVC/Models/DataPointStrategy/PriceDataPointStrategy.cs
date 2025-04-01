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

            public override async Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
            TimeframeOptions timeframeOptions, BearerToken? bearerToken)
            {
                IPriceInfoService priceInfoService = _serviceProvider.GetRequiredService<IPriceInfoService>();

                Result<PriceInfoListDto> result = await priceInfoService.GetPriceInfoAsync(
                        dateTime, timeframeOptions);

                if (!result.IsSuccess || result.Value == null)
                {
                    throw new Exception();
                }

                List<DataPoint> dataPoints = result.Value.PriceInfoList
                    .Select(cr => new DataPoint(cr.Timestamp, cr.PricePerKwh))
                    .ToList();

                List<DataPoint> sortedDatapoints = SortList(dataPoints);

                return sortedDatapoints;
            }

        }
    }
}
