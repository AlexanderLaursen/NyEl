using Common.Enums;

namespace Common.Models
{
    public class PdfFullStatus
    {
        public Guid GUID { get; set; }
        public ServiceStatus Status { get; set; }
        public int QueueLength { get; set; }
        public int Delay { get; set; }
        public bool DelayActive { get; set; }
        public int QueueCheckInterval { get; set; }

    }
}