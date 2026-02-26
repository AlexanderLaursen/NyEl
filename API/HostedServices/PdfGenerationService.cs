using API.Models.PdfGeneration;
using API.Models.PdfGeneration.InvoiceGeneration;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using iText.Html2pdf;

namespace API.HostedServices
{
    public class PdfGenerationService : BackgroundService
    {
        private readonly ILogger<PdfGenerationService> _logger;
        private readonly IPdfGenerationQueue _queue;
        private readonly PdfInvoiceEventHandler _pdfInvoiceEventHandler;

        private Guid Guid;
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

            Guid = Guid.NewGuid();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PdfGenerationService started.");
            _status = ServiceStatus.Running;

            // Loop runs until cancellation token
            while (!stoppingToken.IsCancellationRequested)
            {
                // Tries to take job from queue
                if (_queue.TryTakeJob(out PdfJob job))
                {
                    try
                    {
                        // Generates HTML content
                        HtmlContent htmlContent = job.GenerateHtml();

                        // Converts HTML to PDF
                        Pdf pdf = GeneratePdf(htmlContent);

                        // Raise event
                        // TODO find better solution
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
                // If no job is available, wait for the specified interval
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
            return Guid;
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
                    PdfInvoiceEventArgs invoiceEventArgs = new PdfInvoiceEventArgs(pdf, invoiceJob.Consumer, invoiceJob.Invoice);
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
