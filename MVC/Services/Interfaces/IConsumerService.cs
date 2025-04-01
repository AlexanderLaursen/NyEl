using Common.Dtos.Consumer;
using Common.Models;

namespace MVC.Services.Interfaces
{
    public interface IConsumerService
    {
        public Task<Result<ConsumerDtoFull>> GetConsumerAsync(BearerToken? bearerToken);
    }
}
