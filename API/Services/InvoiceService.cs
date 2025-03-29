using Api.Models;
using API.Models.InvoiceStrategy;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Dtos.Invoice;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Common.Models.TemplateGenerator;
using Mapster;

namespace API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IConsumerRepository _consumerRepository;
        private readonly ILogger<InvoiceService> _logger;
        private readonly InvoiceStrategyContext _invoiceStrategy;
        private readonly TemplateFactory _templateFactory;
        private readonly IPdfGenerationQueue _pdfGenerationQueue;

        public InvoiceService(IInvoiceRepository invoiceRepository, IConsumerRepository consumerRepository,
            InvoiceStrategyContext invoiceStrategy, IPdfGenerationQueue pdfGenerationQueue,
            TemplateFactory templateFactory, IPdfGeneratedNotifier pdfReadyNotifier,
            ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _consumerRepository = consumerRepository;
            _invoiceStrategy = invoiceStrategy;
            _templateFactory = templateFactory;
            _logger = logger;
            _pdfGenerationQueue = pdfGenerationQueue;

            pdfReadyNotifier.PdfGenerated += HandlePdfGenerated;
            _logger.LogInformation("Subscribed");
        }

        public async Task<Invoice> GenerateInvoice(Timeframe fullTimeframe, int consumerId)
        {
            try
            {
                Timeframe timeframe = new Timeframe(fullTimeframe.Start, fullTimeframe.End.AddSeconds(-1));

                bool alreadyExists = await _invoiceRepository.InvoiceExistsAsync(timeframe, consumerId);
                if (alreadyExists)
                {
                    _logger.LogInformation("Invoice already exists for the specified timeframe and consumer.");
                    throw new ServiceException("Invoice already exists for the specified timeframe and consumer.");
                }

                Consumer consumer = await _consumerRepository.GetConsumerByConsumerIdAsync(consumerId);
                if (consumer == null || consumer.BillingModel == null)
                {
                    _logger.LogWarning("Consumer not found.");
                    throw new UnkownUserException("Consumer not found.");
                }

                _invoiceStrategy.SetInvoiceStrategy(consumer.BillingModel.BillingModelType);
                Invoice generatedInvoice = await _invoiceStrategy.GenerateInvoice(timeframe, consumer);

                Invoice invoice = await _invoiceRepository.CreateInvoiceAsync(generatedInvoice, consumerId);

                if (invoice == null)
                {
                    throw new ServiceException();
                }

                PdfGenerationJob job = new PdfGenerationJob
                {
                    Invoice = invoice,
                    Consumer = consumer
                };

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

        public async Task<string> CreateInvoiceHtml(int invoiceId, int consumerId)
        {
            try
            {
                Consumer consumer = await _consumerRepository.GetConsumerByConsumerIdAsync(consumerId);
                Invoice invoice = await _invoiceRepository.GetInvoiceAsync(invoiceId);

                ITemplateGenerator templateGenerator = _templateFactory.CreateTemplateGenerator(TemplateType.Invoice);
                string htmlContent = templateGenerator.GenerateTemplate(invoice, consumer);

                return htmlContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating PDF.");
                throw new ServiceException("Error occurred while generating PDF.", ex);
            }
        }

        public async Task UploadInvoicePdf(int invoiceId, Pdf pdf)
        {
            InvoicePdf invoicePdf = new InvoicePdf
            {
                Id = invoiceId,
                Content = pdf.File,
            };

            await _invoiceRepository.UploadInvoicePdf(invoicePdf);
        }

        public void HandlePdfGenerated(object? sender, PdfGeneratedEventArgs e)
        {
            _logger.LogInformation("test");

            //UploadInvoicePdf(e.InvoiceId, e.Pdf);
        }
    }
}
