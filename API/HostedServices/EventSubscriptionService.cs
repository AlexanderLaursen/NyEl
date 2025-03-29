using API.Services;
using API.Services.Interfaces;

namespace API.HostedServices
{
    public class EventSubscriptionService : IHostedService
    {
        private readonly IPdfGeneratedNotifier _pdfGeneratedNotifier;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventSubscriptionService> _logger;

        public EventSubscriptionService(IPdfGeneratedNotifier pdfGeneratedNotifier, IServiceProvider serviceProvider,
            ILogger<EventSubscriptionService> logger)
        {
            _pdfGeneratedNotifier = pdfGeneratedNotifier;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EventSubscriptionService started.");

            _pdfGeneratedNotifier.PdfGenerated += async (sender, eventArgs) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var invoiceService = scope.ServiceProvider.GetRequiredService<IInvoiceService>();
                    await invoiceService.HandlePdfGenerated(sender, eventArgs);
                }
            };

            _pdfGeneratedNotifier.PdfGenerated += async (sender, eventArgs) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    await notificationService.HandlePdfGenerated(sender, eventArgs);
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
