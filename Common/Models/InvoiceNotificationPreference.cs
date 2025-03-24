using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.Models
{
    public class InvoiceNotificationPreference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public ICollection<ConsumerInvoicePreference> ConsumerPreferences { get; set; }
    }
}
