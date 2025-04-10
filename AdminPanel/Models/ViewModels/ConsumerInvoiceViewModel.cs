using Common.Models;

namespace AdminPanel.Models.ViewModels
{
    public class ConsumerInvoiceViewModel
    {
        public int ConsumerId { get; set; }
        public List<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
