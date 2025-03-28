namespace Common.Dtos.Invoice
{
    public record struct InvoiceDto
    {
        public int Id { get; set; }
        public DateTime BillingPeriodStart { get; set; }
        public DateTime BillingPeriodEnd { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Paid { get; set; }
        public int ConsumerId { get; set; }
        public int BillingModelId { get; set; }
    }
}
