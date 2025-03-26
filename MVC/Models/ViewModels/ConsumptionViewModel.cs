using Common.Enums;

namespace MVC.Models.ViewModels
{
    public class ConsumptionViewModel
    {
        public List<ConsumptionReading> ConsumptionReadings { get; set; }
        public TimeframeOptions SelectedTimeframe { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}
