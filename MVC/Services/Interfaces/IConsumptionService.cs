using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Models;

namespace MVC.Services.Interfaces
{
    public interface IConsumptionService
    {
        Task<Result<ConsumptionReadingListDto>> GetConsumptionReadingsAsync(DateTime startDate, TimeframeOptions timeframeOptions, BearerToken? bearerToken);
    }
}
