using Common.Models;

namespace API.Services.Interfaces
{
    public interface IPdfGeneratedNotifier
    {
        public event EventHandler<PdfGeneratedEventArgs> PdfGenerated;
        public void OnPdfGenerated(object? sender, PdfGeneratedEventArgs eventsArgs);
    }
}
