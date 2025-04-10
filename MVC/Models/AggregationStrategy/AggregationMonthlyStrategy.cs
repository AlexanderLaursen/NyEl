using System.Globalization;
using Common.Models;

namespace MVC.Models.AggregationStrategy
{
    public class AggregationMonthlyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            // Sorts the list into groups by date
            var groupedData = data.GroupBy(dataPoint => dataPoint.Timestamp.Date);
            var aggregatedResults = new AggregatedData();

            // Iterate through each group and apply the operation
            foreach (var group in groupedData)
            {
                // Create label
                string label = group.First().Timestamp.ToString("dd/MM");

                // Calculate the value using the func
                List<DataPoint> dataPoints = group.ToList();
                decimal aggregatedValue = operation(dataPoints);

                // Add the new (graph ready) datapoint
                GraphDataPoint aggregatedDataPoint = new GraphDataPoint(label, aggregatedValue);
                aggregatedResults.GraphDataPoints.Add(aggregatedDataPoint);
            }

            return aggregatedResults;
        }
    }
}
