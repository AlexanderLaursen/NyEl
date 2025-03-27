using System.Globalization;
using Common.Models;

namespace MVC.Models.AggregationStrategy
{
    public class AggregationMonthlyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            CultureInfo danishCulture = new CultureInfo("da-DK");

            var groupedData = data.GroupBy(dataPoint => dataPoint.Timestamp.Date);
            var aggregatedResults = new AggregatedData();

            foreach (var group in groupedData)
            {
                string label = group.First().Timestamp.ToString("dd/MM", danishCulture);

                List<DataPoint> dataPoints = group.ToList();
                decimal aggregatedValue = operation(dataPoints);

                GraphDataPoint aggregatedDataPoint = new GraphDataPoint(label, aggregatedValue);
                aggregatedResults.GraphDataPoints.Add(aggregatedDataPoint);
            }

            return aggregatedResults;
        }
    }
}
