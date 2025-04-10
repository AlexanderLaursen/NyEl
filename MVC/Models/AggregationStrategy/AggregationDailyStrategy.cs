using Common.Models;

namespace MVC.Models.AggregationStrategy
{
    public class AggregationDailyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            AggregatedData aggregatedResults = new AggregatedData();

            // Iterates each datapoint
            foreach (var dataPoint in data)
            {
                // Create label
                string label = dataPoint.Timestamp.ToString("HH:mm");

                // Calculate the value using the func
                List<DataPoint> dataPoints = new List<DataPoint> { dataPoint };
                decimal aggregatedValue = operation(dataPoints);

                // Add the new (graph ready) datapoint
                GraphDataPoint aggregatedDataPoint = new GraphDataPoint(label, aggregatedValue);
                aggregatedResults.GraphDataPoints.Add(aggregatedDataPoint);
            }

            return aggregatedResults;
        }
    }
}