namespace API.Services.Interfaces
{
    public interface IConsumerService
    {
        public Task<int> GetConsumerId(string userId);
        public Task<List<int>> GetAllActiveConsumerIds();
    }
}
