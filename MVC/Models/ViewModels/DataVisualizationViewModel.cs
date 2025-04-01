using Common.Enums;
using Common.Models;

namespace MVC.Models.ViewModels
{
    public class DataVisualizationViewModel
    {
        public AggregatedData AggregatedData { get; set; } = new AggregatedData();
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public TimeframeOptions SelectedTimeframe { get; set; } = TimeframeOptions.Daily;
        public DateTime SelectedDate { get; set; } = new DateTime(2025, 03, 24);
        public RequestedDataType RequestedDataType { get; set; } = RequestedDataType.Consumption;
        public SortingType SortingType { get; set; } = SortingType.Sum;
    }
}
