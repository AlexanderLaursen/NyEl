using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeDailyStrategy : ITimeframeStrategy
    {
        public Timeframe GetTimeframe(DateTime start)
        {
            DateTime startDate = new DateTime(start.Year, start.Month, start.Day);
            DateTime endDate = startDate.AddDays(1).AddSeconds(-1);

            return new Timeframe(startDate, endDate);
        }
    }
}
