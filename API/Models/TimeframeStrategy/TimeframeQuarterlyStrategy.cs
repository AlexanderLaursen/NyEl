using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeQuarterlyStrategy : ITimeframeStrategy
    {
        public Timeframe GetTimeframe(DateTime start)
        {
            int currentMonth = start.Month;

            int startMonth;

            if (currentMonth >= 1 && currentMonth <= 3)
            {
                startMonth = 1;
            }
            else if (currentMonth >= 4 && currentMonth <= 6)
            {
                startMonth = 4;
            }
            else if (currentMonth >= 7 && currentMonth <= 9)
            {
                startMonth = 7;
            }
            else
            {
                startMonth = 10;
            }

            var startDate = new DateTime(start.Year, startMonth, 1);
            var endDate = startDate.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            return new Timeframe(startDate, endDate);
        }
    }
}
