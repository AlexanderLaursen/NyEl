using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class ConsumptionReading
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Consumption { get; set; }

        [ForeignKey("Consumer")]
        [Required]
        public int ConsumerId { get; set; }

        public Consumer Consumer { get; set; }
    }
}
