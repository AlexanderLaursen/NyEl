namespace API.Models.PdfGeneration.InvoiceGeneration
{
    public interface IPdfEventHandler
    {
        public event EventHandler<PdfInvoiceEventArgs> PdfGenerated;
        public void OnPdfGenerated(object? sender, PdfInvoiceEventArgs eventArgs);
    }
}