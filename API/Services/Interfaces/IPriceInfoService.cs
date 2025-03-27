using Common.Enums;
using Common.Models;

namespace API.Services.Interfaces
{
    public interface IPriceInfoService
    {
        public Task<IEnumerable<PriceInfo>> GetPriceInfoAsync(DateTime startDate, TimeframeOptions timeframeOptions);
    }
}
