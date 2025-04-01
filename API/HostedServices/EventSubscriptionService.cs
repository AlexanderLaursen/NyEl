using Api.Models;
using API.Models;
using API.Services;
using API.Services.Interfaces;
using Common.Models;

namespace API.HostedServices
{
    public class EventSubscriptionService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventSubscriptionService> _logger;
        private readonly PdfInvoiceEventHandler _pdfInvoiceEventHandler;

        public EventSubscriptionService(IServiceProvider serviceProvider,
            ILogger<EventSubscriptionService> logger, PdfInvoiceEventHandler pdfInvoiceEventHandler)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _pdfInvoiceEventHandler = pdfInvoiceEventHandler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EventSubscriptionService started.");

            _pdfInvoiceEventHandler.PdfGenerated += async (sender, eventArgs) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var invoiceService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    await invoiceService.HandlePdfGenerated(sender, eventArgs);
                }
            };

            _pdfInvoiceEventHandler.PdfGenerated += async (sender, eventArgs) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var invoiceService = scope.ServiceProvider.GetRequiredService<IInvoiceService>();
                    await invoiceService.HandlePdfGenerated(sender, eventArgs);
                }
            };

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
