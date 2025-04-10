using Common.Enums;
using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IConsumerRepository
    {
        public Task<Consumer> GetConsumerByConsumerIdAsync(int consumerId);
        public Task<int> AddConsumerAsync(Consumer consumer);
        public Task<int> UpdateBillingModelAsync(BillingModelType billingModelMethod, int consumerId);
        public Task<int> GetConsumerIdAsync(string userId);
        public Task<List<int>> GetAllActiveConsumerIdsAsync();
    }
}
