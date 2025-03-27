using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos.ConsumptionReading
{
    public record struct CreateConsumptionReadingDto
    {
        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Consumption { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
