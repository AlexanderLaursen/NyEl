﻿using API.Models.TimeframeStrategy;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Enums;
using Common.Models;
using Common.Exceptions;

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

        public async Task<IEnumerable<ConsumptionReading>> GetConsumptionReadingsAsync(DateTime startDate, TimeframeOptions timeframeOptions, string id)
        {
            _timeframeContext.SetStrategy(timeframeOptions);
            Timeframe timeframe = _timeframeContext.GetTimeframe(startDate);

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
