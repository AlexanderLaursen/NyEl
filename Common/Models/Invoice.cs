using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime BillingPeriodStart { get; set; }

        [Required]
        public DateTime BillingPeriodEnd { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public bool Paid { get; set; }

        [ForeignKey("Consumer")]
        [Required]
        public int ConsumerId { get; set; }

        [ForeignKey("BillingModel")]
        [Required]
        public int BillingModelId { get; set; }

        public BillingModel BillingModel { get; set; }

        public Consumer Consumer { get; set; }
    }
}
