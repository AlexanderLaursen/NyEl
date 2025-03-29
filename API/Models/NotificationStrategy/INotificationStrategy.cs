using Common.Models;

namespace API.Models.NotificationStrategy
{
    public interface INotificationStrategy
    {
        public Task SendNotification(Consumer consumer, Pdf? pdf);
    }
}
