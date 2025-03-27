using Common.Dtos.BillingModel;
using Common.Enums;
using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IConsumerRepository
    {
        public Task<Consumer> GetByUserIdAsync(string id);
        public Task<int> AddConsumerAsync(Consumer consumer);
        public Task<int> UpdateBillingModelAsync(BillingModelDto billingModelDto, string userId);
    }
}
