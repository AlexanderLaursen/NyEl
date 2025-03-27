using Common.Enums;
using Common.Models;

namespace API.Services.Interfaces
{
    public interface IInvoiceService
    {
        public Task<Invoice> GenerateInvoice(Timeframe timeframe, int consumerId);
        public Task<Invoice> GetInvoiceAsync(int invoiceId);
        public Task<List<Invoice>> GetInvoiceListAsync(Timeframe timeframe, int consumerId);
        public Task<bool> DeleteInvoice (int invoiceId);
    }
}
