using System.Globalization;
using Common.Models;

namespace MVC.Models.AggregationStrategy
{
    public class AggregationQuarterlyStrategy : IAggregationStrategy
    {
        public AggregatedData Aggregate(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            // Generate calendar to get first day of the week using Danish culture
            CultureInfo danishCulture = CultureInfo.CreateSpecificCulture("da-DK");     
            var calendar = danishCulture.Calendar;

            // Group data by week using the calendar
            var groupedData = data.GroupBy(dataPoint => calendar.GetWeekOfYear(dataPoint.Timestamp, CalendarWeekRule.FirstDay, DayOfWeek.Monday));

            var aggregatedResults = new AggregatedData();

            foreach (var group in groupedData)
            {
                // Create label (week number)
                string label = calendar.GetWeekOfYear(group.First().Timestamp, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString();

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
