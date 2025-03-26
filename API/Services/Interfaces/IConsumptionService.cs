using Common.Enums;
using Common.Models;

namespace API.Services.Interfaces
{
    public interface IConsumptionService
    {
        public Task<IEnumerable<ConsumptionReading>> GetConsumptionReadingsAsync(DateTime startDate, TimeframeOptions timeframe, string id);
    }
}
