using Common.Dtos.Consumer;
using Common.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class ConsumerService : CommonApiService, IConsumerService
    {
        const string CONSUMER = "/consumers";
        public ConsumerService(HttpClient httpClient, ILogger<CommonApiService> logger, IConfiguration configuration) : base(httpClient, logger, configuration)
        {
        }

        public async Task<Result<ConsumerDtoFull>> GetConsumerAsync(BearerToken? bearerToken)
        {
            return await GetAsync<ConsumerDtoFull>($"{CONSUMER}", bearerToken);
        }
    }
}
