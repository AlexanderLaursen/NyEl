using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.Models
{
    public class InvoicePreference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public InvoiceNotificationPreference InvoiceNotificationPreference { get; set; }

        public ICollection<ConsumerInvoicePreference> ConsumerPreferences { get; set; }
    }
}
