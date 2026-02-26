using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeMonthlyStrategy : ITimeframeStrategy
    {
        // Returns a Timeframe object representing a month

        public Timeframe GetTimeframe(DateTime start)
        {
            DateTime startDate = new DateTime(start.Year, start.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            return new Timeframe(startDate, endDate);
        }
    }
}
