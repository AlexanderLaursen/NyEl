using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Exceptions;

namespace API.Services
{
    public class ConsumerService : IConsumerService
    {
        private IConsumerRepository _consumerRepository;

        public ConsumerService(IConsumerRepository consumerRepository)
        {
            _consumerRepository = consumerRepository;
        }

        public async Task<int> GetConsumerId(string userId)
        {
            if (userId == null)
            {
                throw new UnkownUserException("User not found.");
            }

            int consumerId = await _consumerRepository.GetConsumerIdAsync(userId);

            if (consumerId == 0)
            {
                throw new UnkownUserException("User not found.");
            }

            return consumerId;
        }
    }
}
