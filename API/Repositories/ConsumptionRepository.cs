using API.Data;
using API.Repositories.Interfaces;
using Common.Exceptions;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ConsumptionRepository : IConsumptionRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ConsumptionRepository> _logger;

        public ConsumptionRepository(DataContext context, ILogger<ConsumptionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ConsumptionReading>> GetConsumptionReadingsAsync(string id, Timeframe timeframe)
        {
            try
            {
                return await _context.ConsumptionReadings
                    .Where(cr => cr.Timestamp >= timeframe.Start
                    && cr.Timestamp <= timeframe.End
                    && cr.UserId == id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving consumption readings from the database.");
                throw new RepositoryException("Error occurred while retrieving consumption readings from the database.", ex);
            }
        }
    }
}
