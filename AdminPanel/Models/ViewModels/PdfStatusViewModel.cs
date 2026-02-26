using Common.Enums;

namespace AdminPanel.Models.ViewModels
{
    internal class PdfStatusViewModel
    {
        public Guid GUID { get; set; }
        public ServiceStatus Status { get; set; }
        public int QueueLength { get; set; }
        public bool DelayActive { get; set; }
        public int Delay { get; set; }
        public int QueueCheckInterval { get; set; }
    }
}