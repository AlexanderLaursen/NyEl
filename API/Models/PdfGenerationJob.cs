using Common.Models;

namespace Api.Models
{
    public class PdfGenerationJob
    {
        public Invoice Invoice { get; set; }
        public Consumer Consumer { get; set; }
    }
}
