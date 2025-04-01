using API.Models;

namespace Api.Models
{
    public interface IPdfGenerationQueue
    {
        public void AddJob(PdfJob job);
        bool TryTakeJob(out PdfJob job);
        int Count {  get; }
    }
}