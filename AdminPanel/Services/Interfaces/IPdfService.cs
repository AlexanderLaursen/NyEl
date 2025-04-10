using AdminPanel.Models;
using Common.Enums;
using Common.Models;

namespace AdminPanel.Services.Interfaces
{
    public interface IPdfService
    {
        public Task<Result<PdfFullStatus>> GetFullStatusAsync(BearerToken bearerToken);

        public Task<Result<ServiceStatus>> GetStatus(BearerToken bearerToken);

        public Task<Result<int>> GetQueueLength(BearerToken bearerToken);

        public Task<Result<int>> GetTestDelay(BearerToken bearerToken);

        public Task<Result<bool>> SetTestDelay(int delay, BearerToken bearerToken);

        public Task<Result<bool>> GetDelayActive(BearerToken bearerToken);

        public Task<Result<bool>> SetDelayActive(bool delayActive, BearerToken bearerToken);

        public Task<Result<int>> GetQueueCheckInterval(BearerToken bearerToken);

        public Task<Result<bool>> SetQueueCheckInterval(int queueInterval, BearerToken bearerToken);
    }
}
