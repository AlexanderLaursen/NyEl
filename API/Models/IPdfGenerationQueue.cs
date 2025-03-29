namespace Api.Models
{
    public interface IPdfGenerationQueue
    {
        public void AddJob(PdfGenerationJob job);
        bool TryTakeJob(out PdfGenerationJob job);
        int Count {  get; }
    }
}