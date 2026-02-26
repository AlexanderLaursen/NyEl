using Common.Models;

namespace API.Models.NotificationStrategy
{
    public class EmailNotificationStrategy : INotificationStrategy
    {
        private readonly ILogger<EmailNotificationStrategy> _logger;

        public EmailNotificationStrategy(ILogger<EmailNotificationStrategy> logger)
        {
            _logger = logger;
        }

        // Simulates sending a notification to the consumer via email
        public async Task SendNotification(Consumer consumer, Pdf pdf)
        {
            _logger.LogInformation($"Simulate email notification sent to consumer with id: {consumer.Id}. Included pdf");

            await Task.CompletedTask;
        }
    }
}
