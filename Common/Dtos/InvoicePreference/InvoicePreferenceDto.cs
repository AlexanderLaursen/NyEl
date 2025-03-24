using Common.Enums;

namespace Common.Dtos.InvoicePreference
{
    public class InvoicePreferenceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InvoiceNotificationPreference InvoicePreference { get; set; }
    }
}
