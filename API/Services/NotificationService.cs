using System.Threading.Tasks;
using API.Models;
using API.Models.NotificationStrategy;
using API.Services.Interfaces;
using Common.Enums;
using Common.Models;

namespace API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationStrategyFactory _strategyFactory;
        private readonly ILogger<NotificationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public NotificationService(INotificationStrategyFactory strategyFactory, IServiceProvider serviceProvider,
            ILogger<NotificationService> logger)
        {
            _serviceProvider = serviceProvider;
            _strategyFactory = strategyFactory;
            _logger = logger;
        }

        private async Task SendNotifications(Consumer consumer, Pdf pdf)
        {
            try
            {
                if (consumer.InvoicePreferences == null)
                {
                    _logger.LogError("Invoice preferences null. Cannot send notifications");
                    return;
                }

                List<InvoicePreferenceType> preferences = consumer.InvoicePreferences.
                    Select(cip => cip.InvoiceNotificationPreference.InvoicePreferenceType).ToList();

                foreach (InvoicePreferenceType preference in preferences)
                {
                    INotificationStrategy notificationStrategy = _strategyFactory.Create(preference);
                    await notificationStrategy.SendNotification(consumer, pdf);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification to consumer with ID: {consumer.Id}");
            }
        }

        public async Task HandlePdfGenerated(object? sender, PdfGeneratedEventArgs eventArgs)
        {
            await SendNotifications(eventArgs.Consumer, eventArgs.Pdf);
        }
    }
}
