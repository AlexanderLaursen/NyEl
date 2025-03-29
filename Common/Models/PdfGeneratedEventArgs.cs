namespace Common.Models
{
    public class PdfGeneratedEventArgs(int invoiceId, Consumer consumer, Pdf pdf)
    {
        public int InvoiceId { get; set; } = invoiceId;
        public Consumer Consumer { get; set; } = consumer;
        public Pdf Pdf { get; set; } = pdf;

    }
}
