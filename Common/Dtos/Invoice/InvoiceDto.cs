using Common.Models;

namespace Common.Dtos.Invoice
{
    public record struct InvoiceDto
    {
        public int Id { get; set; }
        public DateTime BillingPeriodStart { get; set; }
        public DateTime BillingPeriodEnd { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalConsumption { get; set; }
        public bool Paid { get; set; }
        public int ConsumerId { get; set; }
        public Common.Models.BillingModel BillingModel { get; set; }
        public List<InvoicePeriodDto> InvoicePeriodData { get; set; }

    }
}
