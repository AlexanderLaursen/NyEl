using Common.Models;

namespace API.Models
{
    public interface IHandlePdfGenerated
    {
        public Task HandlePdfGenerated(object? sender, PdfInvoiceEventArgs eventArgs);
    }
}
