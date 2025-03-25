using System.ComponentModel.DataAnnotations;

namespace MVC.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(0100000000, 9999999999)]
        public int CPR { get; set; }
    }
}
