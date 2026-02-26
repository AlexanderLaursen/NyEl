using Common.Models;

namespace API.Models.NotificationStrategy
{
    public class EboksNotificationStrategy : INotificationStrategy
    {
        private readonly ILogger<EboksNotificationStrategy> _logger;

        public EboksNotificationStrategy(ILogger<EboksNotificationStrategy> logger)
        {
            _logger = logger;
        }

        // Simulates sending a notification to the consumer via e-boks
        public async Task SendNotification(Consumer consumer, Pdf pdf)
        {
            _logger.LogInformation($"Simulate e-boks notification sent to consumer with id: {consumer.Id}. Included pdf");

            await Task.CompletedTask;
        }
    }
}
