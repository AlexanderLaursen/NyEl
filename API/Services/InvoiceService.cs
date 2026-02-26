using API.Models.InvoiceStrategy;
using API.Models.PdfGeneration.InvoiceGeneration;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Dtos.Invoice;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Mapster;

namespace API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IConsumerRepository _consumerRepository;
        private readonly ILogger<InvoiceService> _logger;
        private readonly InvoiceStrategyContext _invoiceStrategy;
        private readonly IPdfGenerationQueue _pdfGenerationQueue;

        public InvoiceService(IInvoiceRepository invoiceRepository, IConsumerRepository consumerRepository,
            InvoiceStrategyContext invoiceStrategy, IPdfGenerationQueue pdfGenerationQueue,
            ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _consumerRepository = consumerRepository;
            _invoiceStrategy = invoiceStrategy;
            _logger = logger;
            _pdfGenerationQueue = pdfGenerationQueue;
        }

        // Generate an invoice for a consumer based on the provided timeframe
        public async Task<Invoice> GenerateInvoice(Timeframe fullTimeframe, int consumerId)
        {
            try
            {
                // Fixes timeframe
                Timeframe timeframe = new Timeframe(fullTimeframe.Start, fullTimeframe.End.AddSeconds(-1));

                // Check if invoice already exists
                bool alreadyExists = await _invoiceRepository.InvoiceExistsAsync(timeframe, consumerId);
                if (alreadyExists)
                {
                    _logger.LogInformation("Invoice already exists for the specified timeframe and consumer.");
                    throw new ServiceException("Invoice already exists for the specified timeframe and consumer.");
                }

                // Get consumer including billing model
                Consumer consumer = await _consumerRepository.GetConsumerByConsumerIdAsync(consumerId);
                if (consumer == null || consumer.BillingModel == null)
                {
                    _logger.LogWarning("Consumer not found.");
                    throw new UnkownUserException("Consumer not found.");
                }

                // Set the invoice strategy based on the billing model type
                _invoiceStrategy.SetInvoiceStrategy(consumer.BillingModel.BillingModelType);

                // Generate invoice using the selected strategy
                Invoice generatedInvoice = await _invoiceStrategy.GenerateInvoice(timeframe, consumer);

                // Saves the invoice to the DB
                Invoice invoice = await _invoiceRepository.CreateInvoiceAsync(generatedInvoice, consumerId);

                if (invoice == null)
                {
                    throw new ServiceException();
                }

                // Creates a job for the PDF generation queue
                PdfInvoiceJob job = new PdfInvoiceJob
                {
                    Invoice = invoice,
                    Consumer = consumer
                };

                // Adds the job to the async queue
                _pdfGenerationQueue.AddJob(job);

                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating invoice.");
                throw new ServiceException("Error occurred while generating invoice.", ex);
            }
        }

        public Task<Invoice> GetInvoiceAsync(int invoiceId)
        {
            return _invoiceRepository.GetInvoiceAsync(invoiceId);
        }

        public async Task<List<Invoice>> GetInvoiceListAsync(Timeframe timeframe, int consumerId)
        {
            return await _invoiceRepository.GetInvoiceListAsync(timeframe, consumerId);
        }

        public async Task<bool> DeleteInvoice(int invoiceId)
        {
            return await _invoiceRepository.DeleteInvoiceAsync(invoiceId);
        }

        public Task<List<Invoice>> GetInvoicesByIdAsync(int consumerId)
        {
            return _invoiceRepository.GetInvoicesByIdAsync(consumerId);
        }

        // Returns a downloadable PDF file for the specified invoice
        public async Task<Pdf> GetPdfAsync(int consumerId, int invoiceId)
        {
            Invoice invoice = await _invoiceRepository.GetInvoiceAsync(invoiceId);

            if (invoice == null)
            {
                throw new ArgumentException("Invoice not found.");
            }

            if (invoice.ConsumerId != consumerId)
            {
                throw new UnauthorizedAccessException("User is not authorized to view this pdf");
            }

            InvoicePdf invoicePdf = await _invoiceRepository.GetPdfAsync(invoiceId);

            Pdf pdf = new Pdf(invoicePdf.Content);

            return pdf;
        }

        public async Task<Pdf> GetPdfAdminAsync(int invoiceId)
        {
            InvoicePdf invoicePdf = await _invoiceRepository.GetPdfAsync(invoiceId);

            Pdf pdf = new Pdf(invoicePdf.Content);

            return pdf;
        }

        public async Task UploadInvoicePdf(int invoiceId, Pdf pdf)
        {
            InvoicePdf invoicePdf = new InvoicePdf
            {
                InvoiceId = invoiceId,
                Content = pdf.File,
            };

            await _invoiceRepository.UploadInvoicePdf(invoicePdf);
        }

        // Handles the event when a PDF is generated
        public async Task HandlePdfGenerated(object? sender, PdfInvoiceEventArgs e)
        {
            await UploadInvoicePdf(e.Invoice.Id, e.Pdf);
        }
    }
}
