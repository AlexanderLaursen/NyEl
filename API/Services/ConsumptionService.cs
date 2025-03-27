using API.Models.TimeframeStrategy;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Enums;
using Common.Models;
using Common.Exceptions;
using Common.Dtos.ConsumptionReading;
using Mapster;

namespace API.Services
{
    public class ConsumptionService : IConsumptionService
    {
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly ILogger<ConsumptionService> _logger;
        private readonly TimeframeContext _timeframeContext;

        public ConsumptionService(IConsumptionRepository consumptionRepository, ILogger<ConsumptionService> logger, TimeframeContext timeframeContext)
        {
            _consumptionRepository = consumptionRepository;
            _logger = logger;
            _timeframeContext = timeframeContext;
        }

        public async Task<ConsumptionReadingListDto> GetConsumptionReadingsAsync(DateTime startDate, TimeframeOptions timeframeOptions, string id)
        {
            _timeframeContext.SetStrategy(timeframeOptions);
            Timeframe timeframe = _timeframeContext.GetTimeframe(startDate);

            try
            {
                var result = await _consumptionRepository.GetConsumptionReadingsAsync(id, timeframe);

                ConsumptionReadingListDto consumptionReadingListDto = new()
                {
                    ConsumptionReadings = result.Adapt<IEnumerable<ConsumptionReadingDto>>(),
                    Timeframe = timeframe
                };

                return consumptionReadingListDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving consumption readings from the database.");
                throw new ServiceException("Error occurred while retrieving consumption readings from the database.", ex);
            }
        }
    }
}
