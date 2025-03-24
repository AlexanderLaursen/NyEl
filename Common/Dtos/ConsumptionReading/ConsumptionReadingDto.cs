namespace Common.Dtos.ConsumptionReading
{
    public record struct ConsumptionReadingDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Consumption { get; set; }
        public int ConsumerId { get; set; }
    }
}
