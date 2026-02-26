using Common.Enums;

namespace API.Models.NotificationStrategy
{
    public class NotificationStrategyFactory : INotificationStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Creates a notification strategy based on the invoice preference type using dependency injection
        public INotificationStrategy Create(InvoicePreferenceType invoicePreferenceType)
        {
            return invoicePreferenceType switch
            {
                InvoicePreferenceType.Email => _serviceProvider.GetRequiredService<EmailNotificationStrategy>(),
                InvoicePreferenceType.Sms => _serviceProvider.GetRequiredService<SmsNotificationStrategy>(),
                InvoicePreferenceType.Eboks => _serviceProvider.GetRequiredService<EboksNotificationStrategy>(),
                InvoicePreferenceType.Postal => _serviceProvider.GetRequiredService<PostalNotificationStrategy>(),
                _ => throw new ArgumentException($"Unsupported notification type: {invoicePreferenceType}", nameof(invoicePreferenceType)),
            };
        }
    }
}
