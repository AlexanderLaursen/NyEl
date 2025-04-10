using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeQuarterlyStrategy : ITimeframeStrategy
    {
        // Gets the quarter from the start date and returns a Timeframe object representing the whole quarter
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
            var endDate = startDate.AddMonths(3).AddSeconds(-1);
            return new Timeframe(startDate, endDate);
        }
    }
}
