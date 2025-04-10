using Api.Models;
using API.HostedServices.Interfaces;
using API.Models;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using iText.Html2pdf;

namespace API.HostedServices
{
    public class PdfGenerationService : BackgroundService, IPdfGenerationService
    {
        private readonly ILogger<PdfGenerationService> _logger;
        private readonly IPdfGenerationQueue _queue;
        private readonly PdfInvoiceEventHandler _pdfInvoiceEventHandler;

        private Guid guid;
        private ServiceStatus _status = ServiceStatus.Stopped;
        private int queueCheckInterval = 5000;
        private int delay = 5000;
        private bool delayActive;

        public PdfGenerationService(IPdfGenerationQueue queue,
            ILogger<PdfGenerationService> logger,
            PdfInvoiceEventHandler pdfInvoiceEventHandler)
        {
            _logger = logger;
            _queue = queue;
            _pdfInvoiceEventHandler = pdfInvoiceEventHandler;

            guid = Guid.NewGuid();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PdfGenerationService started.");
            _status = ServiceStatus.Running;

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queue.TryTakeJob(out PdfJob job))
                {
                    try
                    {
                        HtmlContent htmlContent = job.GenerateHtml();
                        Pdf pdf = GeneratePdf(htmlContent);

                        RaisePdfGeneratedEvent(job, pdf);

                        if (delayActive)
                        {
                            _logger.LogInformation("Delay applied");
                            await Task.Delay(delay, stoppingToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _status = ServiceStatus.Stopped;
                        _logger.LogInformation("PdfGenerationService stopped by cancellation token.");
                    }
                    catch (Exception ex)
                    {
                        _status = ServiceStatus.Error;
                        _logger.LogError(ex, "Error processing PDF generation job.");
                    }
                }
                else
                {
                    await Task.Delay(queueCheckInterval, stoppingToken);
                }
            }

            _logger.LogInformation("PdfGenerationService is stopping.");
        }

        public ServiceStatus GetStatus()
        {
            return _status;
        }

        public Guid GetGuid()
        {
            return guid;
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

        public void SetQueueCheckInterval(int interval)
        {
            queueCheckInterval = interval;
        }

        public int GetQueueCheckInterval()
        {
            return queueCheckInterval;
        }

        private void RaisePdfGeneratedEvent(PdfJob job, Pdf pdf)
        {
            switch (job)
            {
                case PdfInvoiceJob invoiceJob:
                    var invoiceEventArgs = new PdfInvoiceEventArgs(pdf, invoiceJob.Consumer, invoiceJob.Invoice);
                    _pdfInvoiceEventHandler.OnPdfGenerated(this, invoiceEventArgs);
                    break;

                default:
                    throw new ArgumentException("Event does not exists for this type of pdf job");
            }
        }

        public Pdf GeneratePdf(HtmlContent content)
        {
            try
            {
                string htmlContent = content.Content;

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
