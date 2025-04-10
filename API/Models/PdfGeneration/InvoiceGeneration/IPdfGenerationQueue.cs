namespace API.Models.PdfGeneration.InvoiceGeneration
{
    public interface IPdfGenerationQueue
    {
        public void AddJob(PdfJob job);
        bool TryTakeJob(out PdfJob job);
        int Count {  get; }
    }
}