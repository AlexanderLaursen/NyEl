using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class ConsumerInvoicePreference
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Consumer")]
        [Required]
        public int ConsumerId { get; set; }

        public Consumer Consumer { get; set; }

        [ForeignKey("InvoiceNotificationPreference")]
        [Required]
        public int InvoiceNotificationPreferenceId { get; set; }

        public InvoicePreference InvoiceNotificationPreference { get; set; }
    }
}
