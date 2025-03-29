using Common.Dtos.Invoice;
using Common.Models;

namespace MVC.Services.Interfaces
{
    public interface IInvoiceService
    {
        public Task<Result<List<Invoice>>> GetInvoicesAsync(string bearerToken);
        public Task<Result<InvoiceDto>> GetInvoiceByIdAsync(int id, string bearerToken);
        public Task<Result<byte[]>> GetPdfAsync(int id, string bearerToken);
    }
}
