using API.Repositories.Interfaces;
using Common.Dtos.Consumer;
using Common.Exceptions;
using Common.Models;

namespace API.Models.InvoiceStrategy
{
    public class FixedPriceInvoiceStrategy : IInvoiceStrategy
    {
        private readonly IPriceInfoRepository _priceRepository;
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly ILogger<FixedPriceInvoiceStrategy> _logger;

        public FixedPriceInvoiceStrategy(IPriceInfoRepository priceInfoRepository, IConsumptionRepository consumptionRepository,
            ILogger<FixedPriceInvoiceStrategy> logger)
        {
            _priceRepository = priceInfoRepository;
            _consumptionRepository = consumptionRepository;
            _logger = logger;
        }

        public async Task<Invoice> GenerateInvoice(Timeframe timeframe, Consumer consumer)
        {
            List<ConsumptionReading> consumptionReadings = await _consumptionRepository.GetConsumptionAsync(consumer.Id, timeframe);
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

            decimal fixedPrice = fixedPriceInfo.FixedPrice;

            consumptionReadings.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

            List<List<ConsumptionReading>> consumptionByPeriod = consumptionReadings
                .GroupBy(p => p.Timestamp.Month)
                .Select(group => group.ToList())
                .ToList();

            List<InvoicePeriodData> periodInvoiceData = new List<InvoicePeriodData>();

            for (int i = 0; i < consumptionByPeriod.Count; i++)
            {
                decimal periodCost = consumptionByPeriod[i].Select(c => c.Consumption * fixedPrice).Sum();
                decimal periodConsumption = consumptionByPeriod[i].Select(c => c.Consumption).Sum();
                DateTime periodStart = consumptionByPeriod[i][0].Timestamp.Date;
                DateTime periodEnd = consumptionByPeriod[i][consumptionByPeriod[i].Count - 1].Timestamp.Date;

                periodInvoiceData.Add(new InvoicePeriodData
                {
                    PeriodStart = periodStart,
                    PeriodEnd = periodEnd,
                    Consumption = periodConsumption,
                    Cost = periodCost
                });
            }

            decimal totalCost = 0;
            decimal totalConsumption = 0;

            foreach (var period in periodInvoiceData)
            {
                totalCost += period.Cost;
                totalConsumption += period.Consumption;
            }

            Invoice invoice = new()
            {
                BillingPeriodStart = timeframe.Start,
                BillingPeriodEnd = timeframe.End,
                TotalAmount = totalCost,
                TotalConsumption = totalConsumption,
                Paid = false,
                ConsumerId = consumer.Id,
                BillingModelId = consumer.BillingModelId,
                InvoicePeriodData = periodInvoiceData
            };

            return invoice;
        }
    }
}
