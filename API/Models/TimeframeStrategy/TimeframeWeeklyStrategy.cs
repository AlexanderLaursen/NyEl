using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeWeeklyStrategy : ITimeframeStrategy
    {
        public Timeframe GetTimeframe(DateTime start)
        {
            DateTime startDate;

            // If sunday is the the day, it will subtract 6 days to get the start of the week
            if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                startDate = new DateTime(start.Year, start.Month, start.Day).AddDays(-6);
            }
            else
            {
                // If not, it will subtract the number of days from the start of the week + 1 (since monday is day 1)
                startDate = new DateTime(start.Year, start.Month, start.Day).AddDays(-(int)start.DayOfWeek + 1);
            }

            DateTime endDate = startDate.AddDays(7).AddSeconds(-1);

            return new Timeframe(startDate, endDate);
        }
    }
}
