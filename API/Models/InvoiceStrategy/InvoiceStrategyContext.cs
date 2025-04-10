using Common.Enums;
using Common.Exceptions;
using Common.Models;

namespace API.Models.InvoiceStrategy
{
    public class InvoiceStrategyContext
    {
        private IInvoiceStrategy _invoiceStrategy;
        private FixedPriceInvoiceStrategy _fixedPriceStrategy;
        private MarketPriceInvoiceStrategy _marketPriceStrategy;

        public InvoiceStrategyContext(FixedPriceInvoiceStrategy fixedPriceStrategy, MarketPriceInvoiceStrategy marketPriceStrategy)
        {
            _fixedPriceStrategy = fixedPriceStrategy;
            _marketPriceStrategy = marketPriceStrategy;
        }

        // Generates invoice for the given consumer and timeframe using the selected strategy
        public Task<Invoice> GenerateInvoice(Timeframe timeframe, Consumer consumer)
        {
            if (_invoiceStrategy == null)
            {
                throw new ArgumentException("Invoice strategy not set.");
            }

            return _invoiceStrategy.GenerateInvoice(timeframe, consumer);
        }

        // Sets the invoice strategy based on the billing model type
        public void SetInvoiceStrategy(BillingModelType billingModelType)
        {
            switch (billingModelType)
            {
                case BillingModelType.FixedPrice:
                    _invoiceStrategy = _fixedPriceStrategy;
                    break;
                case BillingModelType.MarkedPrice:
                    _invoiceStrategy = _marketPriceStrategy;
                    break;
                default:
                    throw new ArgumentException("Invalid billing model type.");
            }
        }
    }
}
