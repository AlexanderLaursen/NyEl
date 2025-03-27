using Common.Dtos.BillingModel;
using Common.Dtos.Consumer;
using Common.Dtos.InvoicePreference;
using Common.Enums;
using Common.Models;

namespace MVC.Services.Interfaces
{
    public interface ISettingsService
    {
        public Task<Result<ConsumerDtoFull>> GetSettingsAsync(string bearerToken);
        public Task<Result<bool>> UpdateSettingsAsync(InvoicePreferenceListDto invoicePreferenceListDto, BillingModelDto billingMethod, string bearerToken);
    }
}
