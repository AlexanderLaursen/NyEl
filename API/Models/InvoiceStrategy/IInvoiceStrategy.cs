using Common.Models;

namespace API.Models.InvoiceStrategy
{
    public interface IInvoiceStrategy
    {
        public Task<Invoice> GenerateInvoice(Timeframe timeframe, Consumer consumer);
    }
}
