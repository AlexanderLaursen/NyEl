namespace Common.Dtos.Invoice
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public DateTime InvoicePeriodStart { get; set; }
        public DateTime InvoicePeriodEnd { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Paid { get; set; }
        public int ConsumerId { get; set; }
        public int BillingModelId { get; set; }
    }
}
