using Common.Enums;
using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeContext
    {
        private ITimeframeStrategy _strategy;

        public TimeframeContext(TimeframeOptions timeframeOptions)
        {
            SetStrategy(timeframeOptions);
        }

        public Timeframe GetTimeframe(DateTime startDateTime)
        {
            return _strategy.GetTimeframe(startDateTime);
        }

        public void SetStrategy(TimeframeOptions timeframeOptions)
        {
            switch (timeframeOptions)
            {
                case TimeframeOptions.Daily:
                    _strategy = new TimeframeDailyStrategy();
                    break;
                case TimeframeOptions.Weekly:
                    _strategy = new TimeframeWeeklyStrategy();
                    break;
                case TimeframeOptions.Monthly:
                    _strategy = new TimeframeMonthlyStrategy();
                    break;
                case TimeframeOptions.Quarterly:
                    _strategy = new TimeframeMonthlyStrategy();
                    break;
                case TimeframeOptions.Yearly:
                    _strategy = new TimeframeYearlyStrategy();
                    break;
                default:
                    _strategy = new TimeframeMonthlyStrategy();
                    break;
            }
        }
    }
}
