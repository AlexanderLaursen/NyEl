using API.Services.Interfaces;
using Common.Models;

namespace API.Models
{
    public class PdfInvoiceEventHandler : IPdfEventHandler
    {
        public event EventHandler<PdfInvoiceEventArgs> PdfGenerated;
        public void OnPdfGenerated(object? sender, PdfInvoiceEventArgs eventArgs)
        {
            PdfGenerated?.Invoke(sender, eventArgs);
        }
    }
}
