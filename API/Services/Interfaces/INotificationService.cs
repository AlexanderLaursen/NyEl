using Common.Models;

namespace API.Services.Interfaces
{
    public interface INotificationService
    {
        public Task HandlePdfGenerated(object? sender, PdfGeneratedEventArgs eventArgs);
    }
}
