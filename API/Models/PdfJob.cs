using Common.Models;

namespace API.Models
{
    public abstract class PdfJob
    {
        public Guid JobId = Guid.NewGuid();
        public abstract HtmlContent GenerateHtml();

    }
}
