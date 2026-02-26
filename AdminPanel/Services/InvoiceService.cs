using AdminPanel.Services.Interfaces;
using Common.Dtos.Invoice;
using Common.Models;
using MVC.Services;

namespace AdminPanel.Services
{
    public class InvoiceService : CommonApiService, IInvoiceService
    {
        public InvoiceService(HttpClient httpClient, ILogger<CommonApiService> logger) : base(httpClient, logger)
        {
        }



        public async Task<Result<InvoiceDto>> GenerateAsync(Timeframe timeframe, int consumerId, BearerToken bearerToken)
        {
            return await PostAsync<InvoiceDto>($"/admin/invoices/generate/{consumerId}", timeframe, bearerToken);
        }

        public async Task<Result<int>> GenerateAllAsync(Timeframe timeframe, BearerToken bearerToken)
        {
            return await PostAsync<int>("/admin/invoices/generate/all", timeframe, bearerToken);
        }

        public async Task<Result<bool>> DeleteInvoiceAsync(int invoiceId, BearerToken bearerToken)
        {
            return await DeleteAsync($"/admin/invoices/{invoiceId}", bearerToken);
        }

        public async Task<Result<List<Invoice>>> GetInvoicesByConsumerIdAsync(int consumerId, BearerToken bearerToken)
        {
            return await GetAsync<List<Invoice>>($"/admin/invoices/consumer/{consumerId}", bearerToken);
        }

        public async Task<Result<InvoiceDto>> GetInvoiceByIdAsync(int invoiceId, BearerToken bearerToken)
        {
            return await GetAsync<InvoiceDto>($"/admin/invoices/{invoiceId}", bearerToken);
        }

        public async Task<Result<byte[]>> GetPdfAsync(int invoiceId, BearerToken bearerToken)
        {
            return await GetPdfAsync($"/admin/invoices/download/{invoiceId}", bearerToken);
        }
    }
}
