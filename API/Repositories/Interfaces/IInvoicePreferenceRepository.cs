using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IInvoicePreferenceRepository
    {
        public Task<List<InvoicePreference>> GetByUserIdAsync(string userId);
        public Task<InvoicePreference> CreateInvoicePreference(InvoicePreference invoicePreference, string userId);
        public Task DeleteInvoicePreference(InvoicePreference invoicePreference, string userId);
    }
}
