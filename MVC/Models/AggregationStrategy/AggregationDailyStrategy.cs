namespace MVC.Models.AggregationStrategy
{
    public class AggregationDailyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            AggregatedData aggregatedResults = new AggregatedData();

            foreach (var dataPoint in data)
            {
                List<DataPoint> dataPoints = new List<DataPoint> { dataPoint };
                decimal aggregatedValue = operation(dataPoints);

                DataPoint aggregatedDataPoint = new DataPoint(dataPoint.Timestamp, aggregatedValue);
                aggregatedResults.DataPoints.Add(aggregatedDataPoint);
            }

            return aggregatedResults;
        }
    }
}