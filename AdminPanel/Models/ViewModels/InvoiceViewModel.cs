using Common.Models;

namespace AdminPanel.Models.ViewModels
{
    public class InvoiceViewModel
    {
        public int ConsumerId { get; set; }
        public int InvoiceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
