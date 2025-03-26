namespace Common.Dtos.ConsumptionReading
{
    public record struct ConsumptionReadingDto
    {
        public DateTime Timestamp { get; set; }
        public decimal Consumption { get; set; }
    }
}
