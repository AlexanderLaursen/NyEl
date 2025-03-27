using API.Data;
using API.Repositories.Interfaces;
using Common.Exceptions;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class PriceInfoRepository : IPriceInfoRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<PriceInfoRepository> _logger;

        public PriceInfoRepository(DataContext context, ILogger<PriceInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PriceInfo>> GetPriceInfoAsync(Timeframe timeframe)
        {
            try
            {
                return await _context.PriceInfos
                    .Where(cr => cr.Timestamp >= timeframe.Start
                    && cr.Timestamp <= timeframe.End)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving price info from the database.");
                throw new RepositoryException("Error occurred while retrieving price info from the database.", ex);
            }
        }
    }
}
