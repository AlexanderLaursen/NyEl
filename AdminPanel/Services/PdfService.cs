using System.Net.NetworkInformation;
using AdminPanel.Models;
using AdminPanel.Services.Interfaces;
using Common.Enums;
using Common.Models;
using MVC.Services;

namespace AdminPanel.Services
{
    public class PdfService : CommonApiService, IPdfService
    {
        public PdfService(HttpClient httpClient, ILogger<CommonApiService> logger) : base(httpClient, logger)
        {
        }

        public async Task<Result<PdfFullStatus>> GetFullStatusAsync(BearerToken bearerToken)
        {
            return await GetAsync<PdfFullStatus>("/admin/pdf-generator/full-status", bearerToken);
        }

        public async Task<Result<ServiceStatus>> GetStatus (BearerToken bearerToken)
        {
            return await GetAsync<ServiceStatus>("/admin/pdf-generator/status", bearerToken);
        }

        public async Task<Result<int>> GetQueueLength(BearerToken bearerToken)
        {
            return await GetAsync<int>("/admin/pdf-generator/queue-length", bearerToken);
        }

        public async Task<Result<int>> GetTestDelay(BearerToken bearerToken)
        {
            return await GetAsync<int>("/admin/pdf-generator/test-delay/time", bearerToken);
        }

        public async Task<Result<bool>> SetTestDelay(int delay, BearerToken bearerToken)
        {
            var Delay = new { Delay = delay }; 

            return await PostAsync<bool>("/admin/pdf-generator/test-delay/time", Delay, bearerToken);
        }

        public async Task<Result<bool>> GetDelayActive(BearerToken bearerToken)
        {
            return await GetAsync<bool>("/admin/pdf-generator/test-delay/active", bearerToken);
        }

        public async Task<Result<bool>> SetDelayActive(bool delayActive, BearerToken bearerToken)
        {
            var active = new { DelayActive = delayActive };

            return await PostAsync<bool>("/admin/pdf-generator/test-delay/active", active, bearerToken);
        }

        public async Task<Result<int>> GetQueueCheckInterval(BearerToken bearerToken)
        {
            return await GetAsync<int>("/admin/pdf-generator/queue-interval", bearerToken);
        }

        public async Task<Result<bool>> SetQueueCheckInterval(int queueInterval, BearerToken bearerToken)
        {
            var interval = new { QueueInterval = queueInterval };

            return await PostAsync<bool>("/admin/pdf-generator/queue-interval", interval, bearerToken);
        }
    }
}
