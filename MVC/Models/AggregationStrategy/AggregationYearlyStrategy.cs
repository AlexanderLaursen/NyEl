namespace MVC.Models.AggregationStrategy
{
    public class AggregationYearlyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            var groupedData = data.GroupBy(dataPoint => dataPoint.Timestamp.Month);
            var aggregatedResults = new AggregatedData();

            foreach (var group in groupedData)
            {
                DateTime timestamp = group.First().Timestamp;
                List<DataPoint> dataPoints = group.ToList();
                decimal aggregatedValue = operation(dataPoints);


                DataPoint aggregatedDataPoint = new DataPoint(timestamp, aggregatedValue);
                aggregatedResults.DataPoints.Add(aggregatedDataPoint);
            }

            return aggregatedResults;
        }
    }
}
