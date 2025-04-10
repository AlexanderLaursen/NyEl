using AdminPanel.Services.Interfaces;
using Common.Dtos.Consumer;
using Common.Models;
using MVC.Services;

namespace AdminPanel.Services
{
    public class ConsumerService : CommonApiService, IConsumerService
    {
        public ConsumerService(HttpClient httpClient, ILogger<CommonApiService> logger) : base(httpClient, logger)
        {
        }

        public async Task<Result<ConsumerDtoFull>> GetConsumerAsync(int consumerId, BearerToken bearerToken)
        {
            return await GetAsync<ConsumerDtoFull>($"/admin/consumers/{consumerId}", bearerToken);
        }
    }
}
