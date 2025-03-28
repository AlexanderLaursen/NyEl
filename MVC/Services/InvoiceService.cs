using Common.Dtos.Invoice;
using Common.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class InvoiceService : CommonApiService, IInvoiceService
    {
        const string INVOICES_URL = "/invoices";

        public InvoiceService(HttpClient httpClient, ILogger<CommonApiService> logger, IConfiguration configuration) : base(httpClient, logger, configuration)
        {
        }

        public async Task<Result<InvoiceDto>> GetInvoiceByIdAsync(int id, string bearerToken)
        {
            return await GetAsync<InvoiceDto>($"{INVOICES_URL}/{id}", bearerToken);
        }

        public async Task<Result<List<Invoice>>> GetInvoicesAsync(string bearerToken)
        {
            return await GetAsync<List<Invoice>>(INVOICES_URL, bearerToken);
        }
    }
}
