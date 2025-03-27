using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.Models
{
    public class BillingModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public BillingModelType BillingModelType { get; set; }
    }
}
