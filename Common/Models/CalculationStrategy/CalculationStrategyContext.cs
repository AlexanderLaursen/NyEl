using Common.Enums;

namespace Common.Models.CalculationStrategy
{
    public class CalculationStrategyContext
    {
        private ICalculationStrategy _calculationStrategy;

        public CalculationStrategyContext()
        {

        }

        public CalculationStrategyContext(BillingModelType billingModel)
        {
            SetStrategy(billingModel);
        }

        public decimal Calculate(CalculationParameters calculationParameters)
        {
            return _calculationStrategy.Calculate(calculationParameters);
        }

        public void SetStrategy(BillingModelType billingModel)
        {
            switch (billingModel)
            {
                case BillingModelType.FixedPrice:
                    _calculationStrategy = new FixedPriceCalculationStrategy();
                    break;
                case BillingModelType.MarkedPrice:
                    _calculationStrategy = new MarkedPriceCalculationStrategy();
                    break;
                default:
                    throw new ArgumentException("Invalid billing strategy.");
            }
        }
    }
}
