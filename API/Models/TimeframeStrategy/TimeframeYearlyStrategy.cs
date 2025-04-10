using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeYearlyStrategy : ITimeframeStrategy
    {
        // Returns a Timeframe object representing a year
        public Timeframe GetTimeframe(DateTime start)
        {
            DateTime startDate = new DateTime(start.Year, 1, 1);
            DateTime endDate = startDate.AddYears(1).AddSeconds(-1);

            return new Timeframe(startDate, endDate);
        }
    }
}
