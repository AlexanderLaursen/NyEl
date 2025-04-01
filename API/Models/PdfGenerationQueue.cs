using System.Collections.Concurrent;
using API.Models;
using Common.Models;

namespace Api.Models
{
    public class PdfGenerationQueue : IPdfGenerationQueue
    {
        public int Count => _queue.Count;

        private readonly ConcurrentQueue<PdfJob> _queue = new ();

        public void AddJob(PdfJob job)
        {
            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }

            _queue.Enqueue(job);
        }

        public bool TryTakeJob(out PdfJob job)
        {
            return _queue.TryDequeue(out job);
        }
    }
}
