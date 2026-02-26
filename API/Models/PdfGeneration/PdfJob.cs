using Common.Models;

namespace API.Models.PdfGeneration
{
    public abstract class PdfJob
    {
        public Guid JobId = Guid.NewGuid();
        public abstract HtmlContent GenerateHtml();

    }
}
