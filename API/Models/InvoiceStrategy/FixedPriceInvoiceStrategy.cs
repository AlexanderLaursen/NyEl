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

        // Generates invoice for the given consumer and timeframe using fixed price strategy
        public async Task<Invoice> GenerateInvoice(Timeframe timeframe, Consumer consumer)
        {
            // Gets consumption information
            List<ConsumptionReading> consumptionReadings = await _consumptionRepository.GetConsumptionAsync(consumer.Id, timeframe);
            if (consumptionReadings == null || consumptionReadings.Count == 0)
            {
                _logger.LogWarning("Consumption readings not found for the specified timeframe.");
                throw new ServiceException("Consumption readings not found for the specified timeframe.");
            }

            // Gets fixed price information
            FixedPriceInfo fixedPriceInfo = await _priceRepository.GetFixedPriceAsync();
            if (fixedPriceInfo == null)
            {
                _logger.LogWarning("Fixed price info not found.");
                throw new ServiceException("Fixed price info not found.");
            }

            decimal fixedPrice = fixedPriceInfo.FixedPrice;

            // Sorts consumption readings by timestamp
            consumptionReadings.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

            // Group consumption readings by month to seperate each monthly period
            List<List<ConsumptionReading>> consumptionByPeriod = consumptionReadings
                .GroupBy(p => p.Timestamp.Month)
                .Select(group => group.ToList())
                .ToList();

            List<InvoicePeriodData> periodInvoiceData = new List<InvoicePeriodData>();

            // Calculate cost and consumption for each period by iterating through the grouped consumption readings
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

            // Calculate total cost and consumption for the invoice
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
