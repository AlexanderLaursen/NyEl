using Common.Enums;
using Common.Models;

namespace MVC.Models.ViewModels
{
    public class DetailedInvoiceViewModel
    {
        public int InvoiceId { get; set; }
        public DateTime BillingPeriodStart { get; set; }
        public DateTime BillingPeriodEnd { get; set; }
        public decimal TotalConsumption { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Paid { get; set; }
        public List<InvoicePeriodData> PeriodData { get; set; }
        public int ConsumerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CPR { get; set; }
        public string PaidStatus { get; set; }
        public BillingModelType BillingModelType { get; set; }
    }
}
