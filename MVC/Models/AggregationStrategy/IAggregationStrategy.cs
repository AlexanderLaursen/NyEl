using Common.Models;

namespace MVC.Models.AggregationStrategy
{
    public interface IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation);
    }
}
