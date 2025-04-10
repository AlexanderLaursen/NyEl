using Common.Dtos.BillingModel;
using Common.Dtos.Consumer;
using Common.Dtos.InvoicePreference;
using Common.Enums;
using Common.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class SettingsService : CommonApiService, ISettingsService
    {
        const string CONSUMER = "/consumers";
        const string CONSUMER_BILLING = "/consumers/update-billing";
        const string INVOICE_PREFERENCES = "/invoice-preferences";

        public SettingsService(HttpClient httpClient, ILogger<CommonApiService> logger) : base(httpClient, logger)
        {
        }

        public async Task<Result<ConsumerDtoFull>> GetSettingsAsync(BearerToken? bearerToken)
        {
            return await GetAsync<ConsumerDtoFull>(CONSUMER, bearerToken);
        }

        public async Task<Result<bool>> UpdateSettingsAsync(InvoicePreferenceListDto invoicePreferenceListDto, BillingModelDto billingMethod, BearerToken? bearerToken)
        {
            Result<bool> resultInvoice = await PostAsync<bool>(INVOICE_PREFERENCES, invoicePreferenceListDto, bearerToken);
            Result<bool> resultBilling = await PostAsync<bool>(CONSUMER_BILLING, billingMethod, bearerToken);

            if (!resultInvoice.IsSuccess || !resultBilling.IsSuccess)
            {
                return Result<bool>.Failure();
            }

            return Result<bool>.Success(true);
        }
    }
}
