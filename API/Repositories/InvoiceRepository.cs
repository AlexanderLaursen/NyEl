using API.Data;
using API.Repositories.Interfaces;
using Common.Exceptions;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<InvoiceRepository> _logger;

        public InvoiceRepository(DataContext context, ILogger<InvoiceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Invoice> GetInvoiceAsync(int invoiceId)
        {
            try
            {
                return await _context.Invoices
                    .Where(i => i.Id == invoiceId)
                    .Include(i => i.InvoicePeriodData)
                    .Include(i => i.BillingModel)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving invoice preference from the database.");
                throw new RepositoryException("Error occurred while retrieving invoice preference from the database.", ex);
            }
        }

        public async Task<List<Invoice>> GetInvoiceListAsync(Timeframe timeframe, int consumerId)
        {
            try
            {
                return await _context.Invoices
                    .Where(i => i.ConsumerId == consumerId &&
                        i.BillingPeriodStart <= timeframe.End &&
                        i.BillingPeriodEnd >= timeframe.Start)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving invoice preference from the database.");
                throw new RepositoryException("Error occurred while retrieving invoice preference from the database.", ex);
            }
        }

        public async Task<bool> InvoiceExistsAsync(Timeframe timeframe, int consumerId)
        {
            try
            {
                return await _context.Invoices
                    .Where(i => i.ConsumerId == consumerId &&
                        i.BillingPeriodStart <= timeframe.End &&
                        i.BillingPeriodEnd >= timeframe.Start)
                    .AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving invoice preference from the database.");
                throw new RepositoryException("Error occurred while retrieving invoice preference from the database.", ex);
            }
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice, int consumerId)
        {
            try
            {
                _context.Invoices.Add(invoice);
                int changes = await _context.SaveChangesAsync();

                if (changes == 0)
                {
                    return null;
                }

                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating invoice in the database.");
                throw new RepositoryException("Error occurred while creating invoice in the database.", ex);
            }
        }

        public async Task<bool> DeleteInvoiceAsync(int invoiceId)
        {
            try
            {
                var invoice = await _context.Invoices
                    .Where(i => i.Id == invoiceId)
                    .FirstOrDefaultAsync();

                if (invoice == null)
                {
                    return false;
                }

                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting invoice from the database.");
                throw new RepositoryException("Error occurred while deleting invoice from the database.", ex);
            }
        }

        public async Task<List<Invoice>> GetInvoicesByIdAsync(int consumerId)
        {
            return await _context.Invoices.Where(i => i.ConsumerId == consumerId).Include(i => i.BillingModel).ToListAsync();
        }

        public async Task<int> UploadInvoicePdf(InvoicePdf pdf)
        {
            try
            {
                _context.InvoicePdfs.Add(pdf);

                int changes = await _context.SaveChangesAsync();

                if (changes == 0)
                {
                    _logger.LogWarning($"Pdf did not upload succesful. InvoiceId: {pdf.InvoiceId}");
                }

                return changes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading invoice PDF to the database.");
                throw new RepositoryException("Error occurred while uploading invoice PDF to the database.", ex);
            }
        }

        public async Task<InvoicePdf> GetPdfAsync(int invoiceId)
        {
            try
            {
                return await _context.InvoicePdfs.FirstOrDefaultAsync(pdf => pdf.InvoiceId == invoiceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving pdf from the database.");
                throw new RepositoryException("Error occurred while retrieving pdf from the database.", ex);
            }
        }
    }
}
