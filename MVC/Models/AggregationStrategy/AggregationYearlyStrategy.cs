using Common.Models;

namespace MVC.Models.AggregationStrategy
{
    public class AggregationYearlyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            // Group the data by month
            var groupedData = data.GroupBy(dataPoint => dataPoint.Timestamp.Month);
            var aggregatedResults = new AggregatedData();

            foreach (var group in groupedData)
            {
                // Create label
                int month = group.First().Timestamp.Month;
                string label = ProduceLabel(month);

                // Calculate the value using the func
                List<DataPoint> dataPoints = group.ToList();
                decimal aggregatedValue = operation(dataPoints);

                // Add the new (graph ready) datapoint
                GraphDataPoint aggregatedDataPoint = new GraphDataPoint(label, aggregatedValue);
                aggregatedResults.GraphDataPoints.Add(aggregatedDataPoint);
            }

            return aggregatedResults;
        }

        // Helper method to retrieve month name
        private string ProduceLabel(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "Maj";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sep";
                case 10:
                    return "Okt";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";
                default:
                    return "";
            }
        }
    }
}
