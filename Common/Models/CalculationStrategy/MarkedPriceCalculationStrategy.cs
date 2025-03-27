namespace Common.Models.CalculationStrategy
{
    public class MarkedPriceCalculationStrategy : ICalculationStrategy
    {
        public decimal Calculate(CalculationParameters calculationParameters)
        {
            if (calculationParameters.ConsumptionDataPoints.Count != calculationParameters.PriceDataPoints.Count)
            {
                throw new ArgumentException("Consumption and price points count mismatch.");
            }

            calculationParameters.ConsumptionDataPoints.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));
            calculationParameters.PriceDataPoints.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

            decimal totalCost = 0;

            for (int i = 0; i < calculationParameters.ConsumptionDataPoints.Count; i++)
            {
                decimal consumption = calculationParameters.ConsumptionDataPoints[i].Value;
                decimal price = calculationParameters.PriceDataPoints[i].Value;
                totalCost += consumption * price;
            }

            return totalCost;
        }
    }
}
