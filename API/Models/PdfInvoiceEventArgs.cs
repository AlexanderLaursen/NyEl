using API.Models;

namespace Common.Models
{
    public class PdfInvoiceEventArgs(Pdf pdf, Consumer consumer, Invoice invoice)
    {
        public Pdf Pdf { get; set; } = pdf;
        public Consumer Consumer { get; set; } = consumer;
        public Invoice Invoice { get; set; } = invoice;

    }
}
