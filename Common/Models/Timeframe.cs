namespace Common.Models
{
    public struct Timeframe(DateTime start, DateTime end)
    {
        public DateTime Start { get; set; } = start;
        public DateTime End { get; set; } = end;
    }
}
