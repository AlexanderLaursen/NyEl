using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IPriceInfoRepository
    {
        public Task<List<PriceInfo>> GetPriceInfoAsync(Timeframe timeframe);
        public Task<FixedPriceInfo> GetFixedPriceAsync();
    }
}
