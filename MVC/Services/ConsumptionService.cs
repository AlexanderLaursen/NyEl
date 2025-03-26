﻿using System.Globalization;
using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class ConsumptionService : CommonApiService, IConsumptionService
    {
        const string CONSUMPTION_READINGS_URL = "/consumption-readings";

        public ConsumptionService(HttpClient httpClient, ILogger<CommonApiService> logger, IConfiguration configuration) : base(httpClient, logger, configuration)
        {
        }

        public async Task<Result<ConsumptionReadingListDto>> GetConsumptionReadingsAsync(DateTime startDate, TimeframeOptions timeframeOptions, string bearerToken)
        {
            string dateString = startDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);

            string url = $"{CONSUMPTION_READINGS_URL}?startDate={dateString}&timeframeOptions={timeframeOptions}";
            return await GetAsync<ConsumptionReadingListDto>(url, bearerToken);
        }
    }
}
