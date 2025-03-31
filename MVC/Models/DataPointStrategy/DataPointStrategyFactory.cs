using Common.Enums;

namespace MVC.Controllers
{
    public partial class DataVisualizationController
    {
        public class DataPointStrategyFactory
        {
            private readonly IServiceProvider _serviceProvider;

            public DataPointStrategyFactory(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public DataPointStrategy Create(RequestedDataType dataType)
            {
                return dataType switch
                {
                    RequestedDataType.Consumption => new ConsumptionDataPointStrategy(_serviceProvider),
                    RequestedDataType.Price => new PriceDataPointStrategy(_serviceProvider),
                    RequestedDataType.Cost => new CostDataPointStrategy(_serviceProvider),
                    _ => throw new ArgumentException()
                };
            }
        }
    }
}
