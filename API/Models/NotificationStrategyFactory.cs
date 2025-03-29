﻿using API.Models.NotificationStrategy;
using Common.Enums;

namespace API.Models
{
    public class NotificationStrategyFactory : INotificationStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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
