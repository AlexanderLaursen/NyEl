using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IConsumptionRepository
    {
        Task<List<ConsumptionReading>> GetConsumptionAsync(int consumerId, Timeframe timeframe);
    }
}
