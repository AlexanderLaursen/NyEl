using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeYearlyStrategy : ITimeframeStrategy
    {
        public Timeframe GetTimeframe(DateTime start)
        {
            DateTime startDate = new DateTime(start.Year, 1, 1);
            DateTime endDate = startDate.AddYears(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

            return new Timeframe(startDate, endDate);
        }
    }
}
