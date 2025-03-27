using Common.Dtos.ConsumptionReading;
using Common.Enums;

namespace API.Services.Interfaces
{
    public interface IConsumptionService
    {
        public Task<ConsumptionReadingListDto> GetConsumptionReadingsAsync(DateTime startDate, TimeframeOptions timeframe, string id);
    }
}
