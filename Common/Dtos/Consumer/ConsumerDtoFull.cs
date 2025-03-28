using Common.Enums;

namespace Common.Dtos.Consumer
{
    public struct ConsumerDtoFull
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public int CPR { get; set; }
        public int Id { get; set; }
        public BillingModelType BillingModel { get; set; }
        public List<InvoicePreferenceType> InvoicePreferences { get; set; }
        public List<Common.Models.Invoice> Invoices { get; set; }
    }
}
