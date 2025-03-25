using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class PriceInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal PricePerKwh { get; set; }
    }
}
