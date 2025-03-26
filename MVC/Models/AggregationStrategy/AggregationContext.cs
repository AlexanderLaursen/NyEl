using Common.Enums;

namespace MVC.Models.AggregationStrategy
{
    public class AggregationContext
    {
        private IAggregationStrategy _strategy;

        public AggregationContext() {}

        public AggregatedData GetAggregatedData(List<DataPoint> data, Func<List<DataPoint>, decimal> operation)
        {
            if (_strategy == null)
            {
                throw new InvalidOperationException("Strategy not set.");
            }

            return _strategy.Aggregate(data, operation);
        }

        public void SetStrategy(TimeframeOptions timeframeOptions)
        {
            switch (timeframeOptions)
            {
                case TimeframeOptions.Daily:
                    _strategy = new AggregationDailyStrategy();
                    break;
                case TimeframeOptions.Weekly:
                    _strategy = new AggregationWeeklyStrategy();
                    break;
                case TimeframeOptions.Monthly:
                    _strategy = new AggregationMonthlyStrategy();
                    break;
                case TimeframeOptions.Quarterly:
                    _strategy = new AggregationMonthlyStrategy();
                    break;
                case TimeframeOptions.Yearly:
                    _strategy = new AggregationYearlyStrategy();
                    break;
            }
        }
    }
}
