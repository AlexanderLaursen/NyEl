namespace Common.Dtos.PriceInfo
{
    public record struct PriceInfoDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal PricePerKwh { get; set; }
    }
}
