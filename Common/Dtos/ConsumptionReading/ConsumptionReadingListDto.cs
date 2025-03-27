using Common.Models;

namespace Common.Dtos.ConsumptionReading
{
    public class ConsumptionReadingListDto
    {
        public IEnumerable<ConsumptionReadingDto> ConsumptionReadings { get; set; }
        public Timeframe Timeframe { get; set; }
    }
}
