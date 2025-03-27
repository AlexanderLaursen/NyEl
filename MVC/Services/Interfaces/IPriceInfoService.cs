using Common.Dtos.PriceInfo;
using Common.Enums;
using Common.Models;

namespace MVC.Services.Interfaces
{
    public interface IPriceInfoService
    {
        public Task<Result<PriceInfoListDto>> GetPriceInfoAsync(DateTime startDate, TimeframeOptions timeframeOptions);
    }
}
