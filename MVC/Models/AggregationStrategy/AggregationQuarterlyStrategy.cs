using System.Globalization;
using Common.Models;

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
                string label = calendar.GetWeekOfYear(group.First().Timestamp, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString();

                List<DataPoint> dataPoints = group.ToList();
                decimal aggregatedValue = operation(dataPoints);


                GraphDataPoint aggregatedDataPoint = new GraphDataPoint(label, aggregatedValue);
                aggregatedResults.GraphDataPoints.Add(aggregatedDataPoint);
            }

            return aggregatedResults;
        }
    }
}
