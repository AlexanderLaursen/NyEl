using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IConsumptionRepository
    {
        public Task<List<ConsumptionReading>> GetConsumptionAsync(int consumerId, Timeframe timeframe);
        public Task<int> AddAsync(ConsumptionReading consumptionReading);
        public Task<int> AddRangeAsync(List<ConsumptionReading> consumptionReading);
    }
}
