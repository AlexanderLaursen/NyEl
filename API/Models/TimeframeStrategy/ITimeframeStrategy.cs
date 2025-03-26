using Common.Models;

namespace API.Models.TimeframeStrategy
{
    public interface ITimeframeStrategy
    {
        public Timeframe GetTimeframe(DateTime start);
    }
}
