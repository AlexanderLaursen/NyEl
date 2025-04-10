using Common.Enums;

namespace API.HostedServices.Interfaces
{
    public interface IPdfGenerationService
    {
        public ServiceStatus GetStatus();

        public int GetQueueLength();

        public void SetDelay(int delay);

        public int GetDelay();

        public void SetDelayActive(bool delayActive);

        public bool GetDelayActive();

        public void SetQueueCheckInterval(int interval);

        public int GetQueueCheckInterval();

        public Guid GetGuid();
    }
}
