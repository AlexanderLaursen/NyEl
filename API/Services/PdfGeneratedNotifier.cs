using API.Services.Interfaces;
using Common.Models;

namespace API.Services
{
    public class PdfGeneratedNotifier : IPdfGeneratedNotifier
    {
        public event EventHandler<PdfGeneratedEventArgs> PdfGenerated;

        public void OnPdfGenerated(object? sender, PdfGeneratedEventArgs eventArgs)
        {
            PdfGenerated?.Invoke(this, eventArgs);
        }
    }
}
