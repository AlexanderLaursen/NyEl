namespace MVC.Models
{
    public struct DataPoint (DateTime timestamp, decimal value)
    {
        public DateTime Timestamp { get; set; } = timestamp;
        public decimal Value { get; set; } = value;
    }
}
