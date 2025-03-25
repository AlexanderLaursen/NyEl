using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.Models
{
    public class Consumer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Range(0100000000, 9999999999)]
        public int CPR { get; set; }

        [ForeignKey("BillingModel")]
        [Required]
        public int BillingModelId { get; set; } = 1;

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }

        public AppUser User { get; set; }
        public BillingModel BillingModel { get; set; }

        public ICollection<ConsumptionReading> ConsumptionReadings { get; set; }
        public ICollection<ConsumerInvoicePreference> InvoicePreferences { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
}
