using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeMonthlyStrategy : ITimeframeStrategy
    {
        public Timeframe GetTimeframe(DateTime start)
        {
            DateTime startDate = new DateTime(start.Year, start.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

            return new Timeframe(startDate, endDate);
        }
    }
}
