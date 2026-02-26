namespace API.Models.PdfGeneration.InvoiceGeneration
{
    public interface IHandlePdfGenerated
    {
        public Task HandlePdfGenerated(object? sender, PdfInvoiceEventArgs eventArgs);
    }
}
