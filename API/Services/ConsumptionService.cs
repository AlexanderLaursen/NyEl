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

        public async Task<ConsumptionReadingListDto> GetConsumptionReadingsAsync(DateTime startDate, TimeframeOptions timeframeOptions, int userId)
        {
            // Decides the timeframe by setting the strategy based on the provided TimeframeOptions
            _timeframeContext.SetStrategy(timeframeOptions);

            // Gets the timeframe based on the start date and selected strategy
            Timeframe timeframe = _timeframeContext.GetTimeframe(startDate);

            try
            {
                // Uses the newly created timeframe to get the consumption readings
                var result = await _consumptionRepository.GetConsumptionAsync(userId, timeframe);

                ConsumptionReadingListDto consumptionReadingListDto = new()
                {
                    ConsumptionReadings = result.Adapt<List<ConsumptionReadingDto>>(),
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

        public async Task AddAsync(ConsumptionReading consumptionReading)
        {
            int changes = await _consumptionRepository.AddAsync(consumptionReading);

            if (changes == 0)
            {
                throw new ServiceException("Error occured while adding to the database");
            }

            await Task.CompletedTask;
        }

        public async Task AddRangeAsync(List<ConsumptionReading> consumptionReadings)
        {
            int changes = await _consumptionRepository.AddRangeAsync(consumptionReadings);

            if (changes == 0)
            {
                throw new ServiceException("Error occured while adding to the database");
            }

            await Task.CompletedTask;
        }
    }
}
