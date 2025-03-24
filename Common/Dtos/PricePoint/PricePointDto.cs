namespace Common.Dtos.PricePoint
{
    public record struct PricePointDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal PricePerKwh { get; set; }
    }
}
