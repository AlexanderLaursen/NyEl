using System.Globalization;

namespace MVC.Models.AggregationStrategy
{
    public class AggregationQuarterlyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            CultureInfo danishCulture = CultureInfo.CreateSpecificCulture("da-DK");     
            var calendar = danishCulture.Calendar;
            var groupedData = data.GroupBy(dataPoint => calendar.GetWeekOfYear(dataPoint.Timestamp, CalendarWeekRule.FirstDay, DayOfWeek.Monday));

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
