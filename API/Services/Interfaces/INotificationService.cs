using Common.Models;

namespace API.Services.Interfaces
{
    public interface INotificationService
    {
        public void HandlePdfGenerated(object? sender, PdfGeneratedEventArgs eventArgs);
    }
}
