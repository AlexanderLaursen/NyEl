using API.Repositories.Interfaces;
using Common.Exceptions;
using Common.Models;

namespace API.Models.InvoiceStrategy
{
    public class MarketPriceInvoiceStrategy : IInvoiceStrategy
    {
        private readonly IPriceInfoRepository _priceRepository;
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly ILogger<MarketPriceInvoiceStrategy> _logger;

        public MarketPriceInvoiceStrategy(IPriceInfoRepository priceInfoRepository, IConsumptionRepository consumptionRepository,
            ILogger<MarketPriceInvoiceStrategy> logger)
        {
            _priceRepository = priceInfoRepository;
            _consumptionRepository= consumptionRepository;
            _logger = logger;
        }

        // Generates invoice for the given consumer and timeframe using market price strategy
        public async Task<Invoice> GenerateInvoice(Timeframe timeframe, Consumer consumer)
        {
            // Gets price information
            List<PriceInfo> priceInfo = await _priceRepository.GetPriceInfoAsync(timeframe);
            if (priceInfo == null || priceInfo.Count == 0)
            {
                _logger.LogWarning("Price info not found for the specified timeframe.");
                throw new ServiceException("Price info not found for the specified timeframe.");
            }

            // Gets consumption information
            List<ConsumptionReading> consumptionReadings = await _consumptionRepository.GetConsumptionAsync(consumer.Id, timeframe);
            if (consumptionReadings == null || consumptionReadings.Count == 0)
            {
                _logger.LogWarning("Consumption readings not found for the specified timeframe.");
                throw new ServiceException("Consumption readings not found for the specified timeframe.");
            }

            // Check if the number of price info and consumption readings match
            if (priceInfo.Count != consumptionReadings.Count)
            {
                _logger.LogWarning("Price info and consumption readings do not match.");
                throw new ServiceException("Price info and consumption readings do not match.");
            }

            // Sorts consumption readings and price info by timestamp
            consumptionReadings.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));
            priceInfo.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

            // Split both into monthly periods
            List<List<ConsumptionReading>> consumptionByPeriod = consumptionReadings
                .GroupBy(p => p.Timestamp.Month)
                .Select(group => group.ToList())
                .ToList();

            List<List<PriceInfo>> priceByPeriod = priceInfo
                .GroupBy(p => p.Timestamp.Month)
                .Select(group => group.ToList())
                .ToList();

            List<InvoicePeriodData> periodInvoiceData = new List<InvoicePeriodData>();

            // Calculate cost and consumption for each period by iterating through the grouped consumption readings
            for (int i = 0; i < consumptionByPeriod.Count; i++)
            {
                decimal periodCost = 0;
                decimal periodConsumption = 0;
                DateTime periodStart = consumptionByPeriod[i][0].Timestamp.Date;
                DateTime periodEnd = consumptionByPeriod[i][consumptionByPeriod[i].Count - 1].Timestamp.Date;

                for (int j = 0; j < consumptionByPeriod[i].Count; j++)
                {
                    periodConsumption += consumptionByPeriod[i][j].Consumption;
                    periodCost += consumptionByPeriod[i][j].Consumption * priceByPeriod[i][j].PricePerKwh;
                }

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

            // Calculate total cost and consumption for the invoice
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
