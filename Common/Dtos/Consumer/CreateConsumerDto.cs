using System.ComponentModel.DataAnnotations;

namespace Common.Dtos.Consumer
{
    public record struct CreateConsumerDto
    {
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

        [Required]
        [Range(0, 2)]
        public int BillingModelId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
