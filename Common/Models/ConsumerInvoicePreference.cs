using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.Models
{
    public class ConsumerInvoicePreference
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }

        public AppUser User { get; set; }

        [ForeignKey("InvoiceNotificationPreference")]
        [Required]
        public int InvoiceNotificationPreferenceId { get; set; }

        public InvoicePreference InvoiceNotificationPreference { get; set; }
    }
}
