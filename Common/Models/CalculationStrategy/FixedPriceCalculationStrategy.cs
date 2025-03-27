namespace Common.Models.CalculationStrategy
{
    public class FixedPriceCalculationStrategy : ICalculationStrategy
    {
        public decimal Calculate(CalculationParameters calculationParameters)
        {
            decimal totalConsumption = 0;

            foreach (var dataPoint in calculationParameters.ConsumptionDataPoints)
            {
                totalConsumption += dataPoint.Value;
            }

            return totalConsumption * calculationParameters.FixedPrice;
        }
    }
}
