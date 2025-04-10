using Common.Dtos.ConsumptionReading;
using System.Globalization;
using Common.Dtos.PriceInfo;
using Common.Enums;
using Common.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class PriceInfoService : CommonApiService, IPriceInfoService
    {
        const string PRICE_INFO_URL = "/price-info";

        public PriceInfoService(HttpClient httpClient, ILogger<CommonApiService> logger) : base(httpClient, logger)
        {
        }

        public async Task<Result<PriceInfoListDto>> GetPriceInfoAsync(DateTime startDate, TimeframeOptions timeframeOptions)
        {
            // Parse datetime to string readable by the API
            string dateString = startDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);

            string url = $"{PRICE_INFO_URL}?startDate={dateString}&timeframeOptions={timeframeOptions}";
            return await GetAsync<PriceInfoListDto>(url);
        }
    }
}
