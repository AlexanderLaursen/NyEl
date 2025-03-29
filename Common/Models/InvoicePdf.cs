using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    public class InvoicePdf
    {
        [Key]
        public int Id { get; set; }

        public byte[] Content { get; set; }

        [Required]
        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
