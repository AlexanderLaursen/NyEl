using Common.Enums;

namespace Common.Dtos.InvoicePreference
{
    public class CreateInvoicePreferenceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InvoiceNotificationPreference InvoicePreference { get; set; }
    }
}
