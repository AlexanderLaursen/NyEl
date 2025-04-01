using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Models;

namespace API.Services.Interfaces
{
    public interface IConsumptionService
    {
        public Task<ConsumptionReadingListDto> GetConsumptionReadingsAsync(DateTime startDate, TimeframeOptions timeframe, int consumerId);
        public Task AddRangeAsync(List<ConsumptionReading> consumptionReadings);
        public Task AddAsync(ConsumptionReading consumptionReading);
    }
}
