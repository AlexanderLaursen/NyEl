using API.Models.NotificationStrategy;
using Common.Enums;

namespace API.Models
{
    public interface INotificationStrategyFactory
    {
        INotificationStrategy Create(InvoicePreferenceType invoicePreferenceType);
    }
}
