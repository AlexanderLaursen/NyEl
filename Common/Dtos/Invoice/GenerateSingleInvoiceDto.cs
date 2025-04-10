using Common.Models;

namespace Common.Dtos.Invoice
{
    public class GenerateSingleInvoiceDto
    {
        public int ConsumerId { get; set; }
        public Timeframe Timeframe { get; set; }
    }
}
