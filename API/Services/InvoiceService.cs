using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Exceptions;
using Common.Models;
using Common.Models.CalculationStrategy;
using Mapster;

namespace API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IConsumerRepository _consumerRepository;
        private readonly IPriceInfoRepository _priceRepository;
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly CalculationStrategyContext _calculationStrategyContext;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(IInvoiceRepository invoiceRepository, IConsumerRepository consumerRepository,
            IPriceInfoRepository priceRepository, IConsumptionRepository consumptionRepository, ILogger<InvoiceService> logger,
            CalculationStrategyContext calculationStrategyContext)
        {
            _invoiceRepository = invoiceRepository;
            _consumerRepository = consumerRepository;
            _priceRepository = priceRepository;
            _consumptionRepository = consumptionRepository;
            _calculationStrategyContext = calculationStrategyContext;
            _logger = logger;
        }

        public async Task<Invoice> GenerateInvoice(Timeframe timeframe, int consumerId)
        {
            try
            {
                bool alreadyExists = await _invoiceRepository.InvoiceExistsAsync(timeframe, consumerId);
                if (alreadyExists)
                {
                    _logger.LogInformation("Invoice already exists for the specified timeframe and consumer.");
                    return default;
                }

                Consumer consumer = await _consumerRepository.GetConsumerByConsumerIdAsync(consumerId);
                if (consumer == null || consumer.BillingModel == null)
                {
                    _logger.LogWarning("Consumer not found.");
                    throw new UnkownUserException("Consumer not found.");
                }

                List<PriceInfo> priceInfo = await _priceRepository.GetPriceInfoAsync(timeframe);
                if (priceInfo == null || priceInfo.Count == 0)
                {
                    _logger.LogWarning("Price info not found for the specified timeframe.");
                    throw new ServiceException("Price info not found for the specified timeframe.");
                }

                List<ConsumptionReading> consumptionReadings = await _consumptionRepository.GetConsumptionAsync(consumerId, timeframe);
                if (consumptionReadings == null || consumptionReadings.Count == 0)
                {
                    _logger.LogWarning("Consumption readings not found for the specified timeframe.");
                    throw new ServiceException("Consumption readings not found for the specified timeframe.");
                }

                FixedPriceInfo fixedPriceInfo = await _priceRepository.GetFixedPriceAsync();
                if (fixedPriceInfo == null)
                {
                    _logger.LogWarning("Fixed price info not found.");
                    throw new ServiceException("Fixed price info not found.");
                }

                List<DataPoint> priceDataPoints = priceInfo.Select(s => new DataPoint(s.Timestamp, s.PricePerKwh)).ToList();
                List<DataPoint> consumptionDataPoints = consumptionReadings.Select(s => new DataPoint(s.Timestamp, s.Consumption)).ToList();

                _calculationStrategyContext.SetStrategy(consumer.BillingModel.BillingModelType);

                CalculationParameters calculationParameters = new()
                {
                    PriceDataPoints = priceDataPoints,
                    ConsumptionDataPoints = consumptionDataPoints,
                    FixedPrice = fixedPriceInfo.FixedPrice
                };

                decimal totalCost = _calculationStrategyContext.Calculate(calculationParameters);

                Invoice invoice = new()
                {
                    BillingPeriodStart = timeframe.Start,
                    BillingPeriodEnd = timeframe.End,
                    TotalAmount = totalCost,
                    Paid = false,
                    BillingModelId = consumer.BillingModelId,
                    ConsumerId = consumerId,
                };

                await _invoiceRepository.CreateInvoiceAsync(invoice, consumerId);

                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating invoice.");
                throw new ServiceException("Error occurred while generating invoice.", ex);
            }
        }

        public Task<Invoice> GetInvoiceAsync(int invoiceId)
        {
            return _invoiceRepository.GetInvoiceAsync(invoiceId);
        }

        public async Task<List<Invoice>> GetInvoiceListAsync(Timeframe timeframe, int consumerId)
        {
            return await _invoiceRepository.GetInvoiceListAsync(timeframe, consumerId);
        }

        public async Task<bool> DeleteInvoice(int invoiceId)
        {
            return await _invoiceRepository.DeleteInvoiceAsync(invoiceId);
        }
    }
}
