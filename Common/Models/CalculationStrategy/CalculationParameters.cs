namespace Common.Models.CalculationStrategy
{
    public class CalculationParameters
    {
        public List<DataPoint> ConsumptionDataPoints { get; set; }
        public List<DataPoint> PriceDataPoints { get; set; }
        public decimal FixedPrice { get; set; }

    }
}
