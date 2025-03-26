using Common.Enums;
using Common.Exceptions;
using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public class TimeframeContext
    {
        private ITimeframeStrategy _strategy;

        public TimeframeContext()
        {
        }

        public Timeframe GetTimeframe(DateTime startDateTime)
        {
            if (_strategy == null)
            {
                throw new NoStrategyException("No strategy selected.");
            }

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
