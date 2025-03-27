using API.Models.TimeframeStrategy;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Enums;
using Common.Exceptions;
using Common.Models;

namespace API.Services
{
    public class PriceInfoService : IPriceInfoService
    {
        private readonly IPriceInfoRepository _priceInfoRepository;
        private readonly ILogger<ConsumptionService> _logger;
        private readonly TimeframeContext _timeframeContext;

        public PriceInfoService(IPriceInfoRepository priceInfoRepository, ILogger<ConsumptionService> logger, TimeframeContext timeframeContext)
        {
            _priceInfoRepository = priceInfoRepository;
            _logger = logger;
            _timeframeContext = timeframeContext;
        }

        public async Task<IEnumerable<PriceInfo>> GetPriceInfoAsync(DateTime startDate, TimeframeOptions timeframeOptions)
        {
            _timeframeContext.SetStrategy(timeframeOptions);
            Timeframe timeframe = _timeframeContext.GetTimeframe(startDate);

            try
            {
                return await _priceInfoRepository.GetPriceInfoAsync(timeframe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving consumption readings from the database.");
                throw new ServiceException("Error occurred while retrieving consumption readings from the database.", ex);
            }
        }
    }
}
