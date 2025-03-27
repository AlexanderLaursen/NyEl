using Common.Models;

namespace API.Repositories.Interfaces
{
    public interface IPriceInfoRepository
    {
        public Task<IEnumerable<PriceInfo>> GetPriceInfoAsync(Timeframe timeframe);
    }
}
