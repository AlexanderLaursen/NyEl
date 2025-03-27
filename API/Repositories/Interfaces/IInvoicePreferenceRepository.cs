using Common.Enums;
using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IInvoicePreferenceRepository
    {
        public Task<List<InvoicePreference>> GetByConsumerIdAsync(int consumerId);
        public Task<InvoicePreference> CreateInvoicePreference(InvoicePreference invoicePreference, int consumerId);
        public Task DeleteInvoicePreference(InvoicePreference invoicePreference, int consumerId);
        public Task<int> UpdateInvoicePreferences(List<InvoicePreferenceType> invoicePreferences, int consumerId);
    }
}
