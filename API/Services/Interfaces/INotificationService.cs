using API.Models.PdfGeneration.InvoiceGeneration;

namespace API.Services.Interfaces
{
    public interface INotificationService
    {
        public Task HandlePdfGenerated(object? sender, PdfInvoiceEventArgs eventArgs);
    }
}
