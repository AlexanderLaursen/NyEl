using Common.Enums;

namespace API.Models.NotificationStrategy
{
    public interface INotificationStrategyFactory
    {
        INotificationStrategy Create(InvoicePreferenceType invoicePreferenceType);
    }
}
