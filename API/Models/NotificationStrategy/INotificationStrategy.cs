using Common.Models;

namespace API.Models.NotificationStrategy
{
    public interface INotificationStrategy
    {
        public void SendNotification(Consumer consumer, Pdf pdf);
    }
}
