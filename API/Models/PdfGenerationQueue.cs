using System.Collections.Concurrent;
using Common.Models;

namespace Api.Models
{
    public class PdfGenerationQueue : IPdfGenerationQueue
    {
        public int Count => _queue.Count;

        private readonly ConcurrentQueue<PdfGenerationJob> _queue = new ();

        public void AddJob(PdfGenerationJob job)
        {
            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }

            _queue.Enqueue(job);
        }

        public bool TryTakeJob(out PdfGenerationJob job)
        {
            return _queue.TryDequeue(out job);
        }
    }
}
