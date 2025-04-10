using Common.Dtos.Consumer;
using Common.Models;

namespace AdminPanel.Services.Interfaces
{
    public interface IConsumerService
    {
        public Task<Result<ConsumerDtoFull>> GetConsumerAsync(int consumerId, BearerToken bearerToken);
    }
}
