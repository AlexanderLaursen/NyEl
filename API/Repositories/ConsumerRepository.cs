using API.Data;
using API.Repositories.Interfaces;
using Common.Dtos.BillingModel;
using Common.Enums;
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

        public async Task<int> GetConsumerIdAsync(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                return await _context.Consumers
                    .Where(c => c.UserId == userId)
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving data from the database.");
                throw new RepositoryException("Error occurred while retrieving data from the database.", ex);
            }
        }

        public async Task<Consumer> GetConsumerByConsumerIdAsync(int consumerId)
        {
            try
            {
                var consumer = await _context.Consumers.Include(c => c.BillingModel)
                    .Include(c => c.InvoicePreferences).ThenInclude(cip => cip.InvoiceNotificationPreference)
                    .FirstOrDefaultAsync(c => c.Id == consumerId);

                if (consumer == null)
                {
                    throw new Exception("Not found.");
                }

                return consumer;
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

        public async Task<int> UpdateBillingModelAsync(BillingModelType billingModelMethod, int consumerId)
        {
            try 
            {
                Consumer? consumer = _context.Consumers.FirstOrDefault(c => c.Id == consumerId);
                if (consumer == null )
                {
                    throw new RepositoryException("Consumer not found.");
                }

                BillingModel? billingModel = _context.BillingModels.FirstOrDefault(b => b.BillingModelType == billingModelMethod);
                if (billingModel == null )
                {
                    throw new RepositoryException("Billing model not found.");
                }

                consumer.BillingModel = billingModel;

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
