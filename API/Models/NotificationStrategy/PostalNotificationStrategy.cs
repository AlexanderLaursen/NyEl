using Common.Models;

namespace API.Models.NotificationStrategy
{
    public class PostalNotificationStrategy : INotificationStrategy
    {
        private readonly ILogger<PostalNotificationStrategy> _logger;

        public PostalNotificationStrategy(ILogger<PostalNotificationStrategy> logger)
        {
            _logger = logger;
        }

        public async Task SendNotification(Consumer consumer, Pdf pdf)
        {
            _logger.LogInformation($"Simulate postal notification sent to consumer with id: {consumer.Id}. Included pdf");

            await Task.CompletedTask;
        }
    }
}
