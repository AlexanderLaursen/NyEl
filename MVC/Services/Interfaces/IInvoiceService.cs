using Common.Dtos.Invoice;
using Common.Models;

namespace MVC.Services.Interfaces
{
    public interface IInvoiceService
    {
        public Task<Result<List<Invoice>>> GetInvoicesAsync(BearerToken? bearerToken);
        public Task<Result<InvoiceDto>> GetInvoiceByIdAsync(int id, BearerToken? bearerToken);
        public Task<Result<byte[]>> GetPdfAsync(int id, BearerToken? bearerToken);
    }
}
