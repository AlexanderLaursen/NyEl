using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IConsumptionRepository
    {
        Task<IEnumerable<ConsumptionReading>> GetConsumptionReadingsAsync(string id, Timeframe timeframe);
    }
}
