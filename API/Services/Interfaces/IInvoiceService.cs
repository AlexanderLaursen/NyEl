using Common.Dtos.Invoice;
using Common.Enums;
using Common.Models;

namespace API.Services.Interfaces
{
    public interface IInvoiceService
    {
        public Task<Invoice> GenerateInvoice(Timeframe timeframe, int consumerId);
        public Task<Invoice> GetInvoiceAsync(int invoiceId);
        public Task<List<Invoice>> GetInvoicesByIdAsync(int consumerId);
        public Task<List<Invoice>> GetInvoiceListAsync(Timeframe timeframe, int consumerId);
        public Task<bool> DeleteInvoice (int invoiceId);
        public Task<string> CreateInvoiceHtml(int invoiceId, int consumerId);
        public Task UploadInvoicePdf(int invoiceId, Pdf pdf);
        public Task HandlePdfGenerated(object? sender, PdfGeneratedEventArgs e);
        public Task<Pdf> GetPdfAsync(int consumerId, int invoiceId);
    }
}
