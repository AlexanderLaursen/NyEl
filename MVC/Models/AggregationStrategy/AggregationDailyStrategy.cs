using Common.Models;

namespace MVC.Models.AggregationStrategy
{
    public class AggregationDailyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            AggregatedData aggregatedResults = new AggregatedData();

            foreach (var dataPoint in data)
            {
                string label = dataPoint.Timestamp.ToString("HH:mm");

                List<DataPoint> dataPoints = new List<DataPoint> { dataPoint };
                decimal aggregatedValue = operation(dataPoints);

                GraphDataPoint aggregatedDataPoint = new GraphDataPoint(label, aggregatedValue);
                aggregatedResults.GraphDataPoints.Add(aggregatedDataPoint);
            }

            return aggregatedResults;
        }
    }
}