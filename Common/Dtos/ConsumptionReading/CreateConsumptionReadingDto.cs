namespace Common.Dtos.ConsumptionReading
{
    public class CreateConsumptionReadingDto
    {
        public DateTime Timestamp { get; set; }
        public decimal Consumption { get; set; }
        public int ConsumerId { get; set; }
    }
}
