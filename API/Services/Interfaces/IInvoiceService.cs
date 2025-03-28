using Common.Dtos.Invoice;
using Common.Enums;
using Common.Models;

namespace API.Services.Interfaces
{
    public interface IInvoiceService
    {
        public Task<InvoiceDto> GenerateInvoice(Timeframe timeframe, int consumerId);
        public Task<Invoice> GetInvoiceAsync(int invoiceId);
        public Task<List<Invoice>> GetInvoicesByIdAsync(int consumerId);
        public Task<List<Invoice>> GetInvoiceListAsync(Timeframe timeframe, int consumerId);
        public Task<bool> DeleteInvoice (int invoiceId);

        public Task<string> CreatePdf(int invoiceId, int consumerId);
    }
}
