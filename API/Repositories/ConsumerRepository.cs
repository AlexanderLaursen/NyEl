using API.Data;
using API.Repositories.Interfaces;
using Common.Dtos.BillingModel;
using Common.Exceptions;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ConsumerRepository : IConsumerRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ConsumerRepository> _logger;

        public ConsumerRepository(DataContext context, ILogger<ConsumerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Consumer> GetByUserIdAsync(string userId)
        {
            try
            {
                return await _context.Consumers.Include(c => c.BillingModel)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving data from the database.");
                throw new RepositoryException("Error occurred while retrieving data from the database.", ex);
            }
        }

        public async Task<int> AddConsumerAsync(Consumer consumer)
        {
            try
            {
                await _context.Consumers.AddAsync(consumer);
                var succesfulWrites = await _context.SaveChangesAsync();
                return succesfulWrites;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while performing write operation to the database");
                throw new RepositoryException("Error occurred while performing write operation to the database", ex);
            }
        }

        public async Task<int> UpdateBillingModelAsync(BillingModelDto billingModelDto, string userId)
        {
            try 
            {
                var consumer = _context.Consumers.FirstOrDefault(c => c.UserId == userId);
                if (consumer == null)
                {
                    throw new RepositoryException("Consumer not found.");
                }
                consumer.BillingModel.BillingModelMethod = billingModelDto.BillingModelMethod;
                _context.Consumers.Update(consumer);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating billing model.");
                throw new RepositoryException("Error occurred while updating billing model.", ex);
            }
        }
    }
}
