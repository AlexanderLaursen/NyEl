using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        public Task<Invoice> GetInvoiceAsync(int invoiceId);
        public Task<List<Invoice>> GetInvoicesByIdAsync(int consumerId);
        public Task<List<Invoice>> GetInvoiceListAsync(Timeframe timeframe, int consumerId);
        public Task<bool> InvoiceExistsAsync(Timeframe timeframe, int consumerId);
        public Task<Invoice> CreateInvoiceAsync(Invoice invoice, int consumerId);
        public Task<bool> DeleteInvoiceAsync(int invoiceId);

        public Task<int> UploadInvoicePdf(InvoicePdf pdf);
    }
}
