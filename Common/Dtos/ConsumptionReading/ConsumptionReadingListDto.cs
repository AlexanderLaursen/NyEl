using Common.Models;

namespace Common.Dtos.ConsumptionReading
{
    public class ConsumptionReadingListDto
    {
        public List<ConsumptionReadingDto> ConsumptionReadings { get; set; }
        public Timeframe Timeframe { get; set; }
    }
}
