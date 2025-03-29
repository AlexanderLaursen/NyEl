namespace Common.Dtos.Invoice
{
    public  class InvoicePeriodDto
    {
        public decimal Cost { get; set; }
        public decimal Consumption { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int InvoiceId { get; set; }
    }
}
