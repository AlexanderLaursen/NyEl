using Common.Dtos.Invoice;
using Common.Models;

namespace AdminPanel.Services.Interfaces
{
    public interface IInvoiceService
    {
        public Task<Result<InvoiceDto>> GenerateAsync(Timeframe timeframe, int consumerId, BearerToken bearerToken);

        public Task<Result<int>> GenerateAllAsync(Timeframe timeframe, BearerToken bearerToken);

        public Task<Result<bool>> DeleteInvoiceAsync(int invoiceId, BearerToken bearerToken);

        public Task<Result<List<Invoice>>> GetInvoicesByConsumerIdAsync(int consumerId, BearerToken bearerToken);

        public Task<Result<InvoiceDto>> GetInvoiceByIdAsync(int invoiceId, BearerToken bearerToken);

        public Task<Result<byte[]>>GetPdfAsync(int invoiceId, BearerToken bearerToken);
    }
}
