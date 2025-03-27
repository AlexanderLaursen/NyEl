using Common.Enums;

namespace MVC.Models.ViewModels
{
    public class SettingsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int CPR { get; set; }
        public BillingModelMethod BillingModel { get; set; }
        public List<InvoicePreferenceEnum> InvoicePreferences { get; set; } = new List<InvoicePreferenceEnum>();
    }
}
