using System.Threading.Tasks;
using API.Models.NotificationStrategy;
using API.Services.Interfaces;
using Common.Enums;
using Common.Models;

namespace API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        private readonly Dictionary<InvoicePreferenceType, INotificationStrategy> _strategies;

        public NotificationService(IPdfGeneratedNotifier pdfGeneratedNotifier, ILogger<NotificationService> logger)
        {
            _logger = logger;

            _strategies = new()
            {
                { InvoicePreferenceType.Email, new EmailNotificationStrategy() },
                { InvoicePreferenceType.Sms, new SmsNotificationStrategy() },
                { InvoicePreferenceType.Eboks, new EboksNotificationStrategy() },
                { InvoicePreferenceType.Postal, new PostalNotificationStrategy() },
            };

            pdfGeneratedNotifier.PdfGenerated += HandlePdfGenerated;
        }

        public void SendNotifications(Consumer consumer, Pdf pdf)
        {
            _logger.LogInformation("test");

            //if (consumer.InvoicePreferences == null)
            //{
            //    _logger.LogError("Invoice preferences null. Cannot send notifications");
            //    return;
            //}

            //List<InvoicePreferenceType> preferences = consumer.InvoicePreferences.
            //    Select(cip => cip.InvoiceNotificationPreference.InvoicePreferenceType).ToList();

            //foreach (InvoicePreferenceType preference in preferences)
            //{ 
            //    if (_strategies.TryGetValue(preference, out INotificationStrategy notificationStrategy))
            //    {
            //        try
            //        {
            //            notificationStrategy.SendNotification(consumer, pdf);
            //        }
            //        catch (Exception ex)
            //        {
            //            _logger.LogError(ex, $"Error sending {preference} notification to consumer with ID: {consumer.Id}");
            //        }
            //    }
            //}  
        }

        public void HandlePdfGenerated(object? sender, PdfGeneratedEventArgs eventArgs)
        {
            SendNotifications(eventArgs.Consumer, eventArgs.Pdf);
        }
    }
}
