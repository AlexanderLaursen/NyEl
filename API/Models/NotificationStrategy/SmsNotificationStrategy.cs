using Common.Models;

namespace API.Models.NotificationStrategy
{
    public class SmsNotificationStrategy : INotificationStrategy
    {
        private readonly ILogger<SmsNotificationStrategy> _logger;

        public SmsNotificationStrategy(ILogger<SmsNotificationStrategy> logger)
        {
            _logger = logger;
        }

        // Simulates sending a notification to the consumer via SMS
        public async Task SendNotification(Consumer consumer, Pdf pdf)
        {
            _logger.LogInformation($"Simulate sms notification sent to consumer with id: {consumer.Id}. Included pdf");

            await Task.CompletedTask;
        }
    }
}
