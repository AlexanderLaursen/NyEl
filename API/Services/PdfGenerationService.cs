using System.Collections.Concurrent;
using Api.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Common.Models.TemplateGenerator;
using iText.Html2pdf;

namespace API.Services
{
    public class PdfGenerationService : BackgroundService
    {
        private readonly ILogger<PdfGenerationService> _logger;
        private readonly IPdfGenerationQueue _queue;
        private readonly IPdfGeneratedNotifier _pdfGeneratedNotifier;
        private readonly TemplateFactory _templateFactory;

        private ServiceStatus _status = ServiceStatus.Stopped;
        private int delay = 5000;
        private bool delayActive;

        public PdfGenerationService(TemplateFactory templateFactory, IPdfGeneratedNotifier pdfGeneratedNotifier,
            IPdfGenerationQueue queue, ILogger<PdfGenerationService> logger)
        {
            _logger = logger;
            _templateFactory = templateFactory;
            _pdfGeneratedNotifier = pdfGeneratedNotifier;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PdfGenerationService started.");
            _status = ServiceStatus.Running;

            while (!stoppingToken.IsCancellationRequested)
            {
               if (_queue.TryTakeJob(out PdfGenerationJob job))
                {
                    try
                    {
                        


                    }
                    catch (OperationCanceledException)
                    {
                        _status = ServiceStatus.Stopped;
                        _logger.LogInformation("PdfGenerationService stopping.");
                    }
                    catch (Exception ex)
                    {
                        _status = ServiceStatus.Error;
                        _logger.LogError(ex, "Error processing PDF generation job.");
                    }
                }
                else
                {
                    byte[] bytes = new byte[1];
                    Consumer consumer = new();
                    Pdf pdf = new(bytes);

                    PdfGeneratedEventArgs eventArgs = new(0, consumer, pdf);

                    _logger.LogInformation("reached?");
                    _pdfGeneratedNotifier?.OnPdfGenerated(this, eventArgs);
                    await Task.Delay(delay, stoppingToken);
                }

                if (delayActive)
                {
                    _logger.LogInformation("Delay applied");
                    await Task.Delay(delay, stoppingToken);
                }
            }

            _logger.LogInformation("PdfGenerationService is stopping.");
        }

        public ServiceStatus GetStatus()
        {
            return _status;
        }

        public int GetQueueLength()
        {
            return _queue.Count;
        }

        public void SetDelay(int delay)
        {
            this.delay = delay;
        }

        public int GetDelay()
        {
            return delay;
        }

        public void SetDelayActive(bool delayActive)
        {
            this.delayActive = delayActive;
        }

        public bool GetDelayActive()
        {
            return delayActive;
        }

        public string CreateInvoiceHtml(Invoice invoice, Consumer consumer)
        {
            try
            {
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

        public Pdf GeneratePdf(string htmlContent)
        {
            try
            {
                byte[] pdfBytes;
                using (var memoryStream = new MemoryStream())
                {
                    HtmlConverter.ConvertToPdf(htmlContent, memoryStream);
                    pdfBytes = memoryStream.ToArray();
                }

                Pdf pdf = new Pdf(pdfBytes);

                return pdf;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating PDF.");
                throw new ServiceException("Error occurred while generating PDF.", ex);
            }
        }
    }
}
