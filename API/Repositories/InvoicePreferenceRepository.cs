using API.Data;
using API.Repositories.Interfaces;
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

        public async Task<List<InvoicePreference>> GetByUserIdAsync(string userId)
        {
            try
            {
                return await _context.ConsumerInvoicePreferences
                    .Where(cip => cip.UserId == userId)
                    .Select(cip => cip.InvoiceNotificationPreference)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving invoice preference from the database.");
                throw new RepositoryException("Error occurred while retrieving invoice preference from the database.", ex);
            }
        }

        public async Task<InvoicePreference> CreateInvoicePreference(InvoicePreference invoicePreference, string userId)
        {
            try
            {
                _context.InvoicePreferences.Add(invoicePreference);
                await _context.SaveChangesAsync();

                ConsumerInvoicePreference consumerInvoicePreference = new ConsumerInvoicePreference
                {
                    UserId = userId,
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

        public async Task DeleteInvoicePreference(InvoicePreference invoicePreference, string userId)
        {
            try
            {
                ConsumerInvoicePreference? consumerInvoicePreference = await _context.ConsumerInvoicePreferences
                    .Where(cip => cip.UserId == userId && cip.InvoiceNotificationPreferenceId == invoicePreference.Id)
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
    }
}
