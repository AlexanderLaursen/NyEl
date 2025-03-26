using API.Models.TimeframeStrategy;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Enums;
using Common.Models;

namespace API.Services
{
    public class ConsumptionService : IConsumptionService
    {
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly ILogger<ConsumptionService> _logger;

        public ConsumptionService(IConsumptionRepository consumptionRepository, ILogger<ConsumptionService> logger)
        {
            _consumptionRepository = consumptionRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ConsumptionReading>> GetConsumptionReadingsAsync(DateTime startDate, TimeframeOptions timeframeOptions, string id)
        {
            TimeframeContext timeframeContext = new(timeframeOptions);
            Timeframe timeframe = timeframeContext.GetTimeframe(startDate);

            try
            {
                return await _consumptionRepository.GetConsumptionReadingsAsync(id, timeframe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving consumption readings from the database.");
                throw new ServiceException("Error occurred while retrieving consumption readings from the database.", ex);
            }
        }
    }
}
