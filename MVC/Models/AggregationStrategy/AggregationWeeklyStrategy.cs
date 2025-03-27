using Common.Models;

namespace MVC.Models.AggregationStrategy
{
    public class AggregationWeeklyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            var groupedData = data.GroupBy(dataPoint => dataPoint.Timestamp.Date);
            var aggregatedResults = new AggregatedData();

            foreach (var group in groupedData)
            {
                DayOfWeek day = group.First().Timestamp.DayOfWeek;
                string label = ProduceLabel(day);

                List<DataPoint> dataPoints = group.ToList();
                decimal aggregatedValue = operation(dataPoints);

                GraphDataPoint aggregatedDataPoint = new GraphDataPoint(label, aggregatedValue);
                aggregatedResults.GraphDataPoints.Add(aggregatedDataPoint);
            }

            return aggregatedResults;
        }

        private string ProduceLabel(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return "Mandag";
                case DayOfWeek.Tuesday:
                    return "Tirsdag";
                case DayOfWeek.Wednesday:
                    return "Onsdag";
                case DayOfWeek.Thursday:
                    return "Torsdag";
                case DayOfWeek.Friday:
                    return "Fredag";
                case DayOfWeek.Saturday:
                    return "Lørdag";
                case DayOfWeek.Sunday:
                    return "Søndag";
                default:
                    return "";
            }
        }
    }
}
