using API.Data;
using API.Repositories.Interfaces;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class InvoicePreferenceRepository : IInvoicePreferenceRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<InvoicePreferenceRepository> _logger;

        public InvoicePreferenceRepository(DataContext context, ILogger<InvoicePreferenceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<InvoicePreference>> GetByConsumerIdAsync(int consumerId)
        {
            try
            {
                return await _context.ConsumerInvoicePreferences
                    .Where(cip => cip.ConsumerId == consumerId)
                    .Select(cip => cip.InvoiceNotificationPreference)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving invoice preference from the database.");
                throw new RepositoryException("Error occurred while retrieving invoice preference from the database.", ex);
            }
        }

        public async Task<InvoicePreference> CreateInvoicePreference(InvoicePreference invoicePreference, int consumerId)
        {
            try
            {
                _context.InvoicePreferences.Add(invoicePreference);
                await _context.SaveChangesAsync();

                ConsumerInvoicePreference consumerInvoicePreference = new ConsumerInvoicePreference
                {
                    ConsumerId = consumerId,
                    InvoiceNotificationPreferenceId = invoicePreference.Id
                };
                _context.ConsumerInvoicePreferences.Add(consumerInvoicePreference);
                await _context.SaveChangesAsync();

                return invoicePreference;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating invoice preference in the database.");
                throw new RepositoryException("Error occurred while creating invoice preference in the database.", ex);
            }
        }

        public async Task DeleteInvoicePreference(InvoicePreference invoicePreference, int consumerId)
        {
            try
            {
                ConsumerInvoicePreference? consumerInvoicePreference = await _context.ConsumerInvoicePreferences
                    .Where(cip => cip.ConsumerId == consumerId && cip.InvoiceNotificationPreferenceId == invoicePreference.Id)
                    .FirstOrDefaultAsync();
                if (consumerInvoicePreference == null)
                {
                    throw new RepositoryException("Invoice preference not found for the specified user.");
                }

                _context.ConsumerInvoicePreferences.Remove(consumerInvoicePreference);
                await _context.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting invoice preference from the database.");
                throw new RepositoryException("Error occurred while deleting invoice preference from the database.", ex);
            }
        }

        public async Task<int> UpdateInvoicePreferences(List<InvoicePreferenceType> invoicePreferences, int consumerId)
        {
            try
            {
                // Gets and removes all old invoice preferences for the consumer
                List<ConsumerInvoicePreference> oldConsumerInvoicePreferences = await _context.ConsumerInvoicePreferences
                    .Where(cip => cip.ConsumerId == consumerId)
                    .ToListAsync();

                _context.ConsumerInvoicePreferences.RemoveRange(oldConsumerInvoicePreferences);

                // Adds each new invoice preference for the consumer
                List<InvoicePreference> allInvoicePreferences = await _context.InvoicePreferences.ToListAsync();

                foreach (InvoicePreferenceType invoicePreferenceEnum in invoicePreferences)
                {
                    InvoicePreference? invoicePreference = allInvoicePreferences
                        .Where(ip => ip.InvoicePreferenceType == invoicePreferenceEnum)
                        .FirstOrDefault();

                    if (invoicePreference == null)
                    {
                        throw new ArgumentException("Invalid invoice preference.");
                    }

                    ConsumerInvoicePreference consumerInvoicePreference = new ConsumerInvoicePreference
                    {
                        ConsumerId = consumerId,
                        InvoiceNotificationPreferenceId = invoicePreference.Id
                    };

                    _context.ConsumerInvoicePreferences.Add(consumerInvoicePreference);
                }

                int changes = await _context.SaveChangesAsync();

                return changes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating invoice preferences in the database.");
                throw new RepositoryException("Error occurred while updating invoice preferences in the database.", ex);
            }
        }
    }
}
