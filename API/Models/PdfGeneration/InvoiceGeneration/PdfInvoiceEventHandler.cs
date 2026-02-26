using API.Services.Interfaces;

namespace API.Models.PdfGeneration.InvoiceGeneration
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
