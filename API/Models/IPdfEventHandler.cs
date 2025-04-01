using Common.Models;

namespace API.Models
{
    public interface IPdfEventHandler
    {
        public event EventHandler<PdfInvoiceEventArgs> PdfGenerated;
        public void OnPdfGenerated(object? sender, PdfInvoiceEventArgs eventArgs);
    }
}