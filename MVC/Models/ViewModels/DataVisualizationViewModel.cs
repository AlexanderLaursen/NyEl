﻿using Common.Enums;
using Common.Models;

namespace MVC.Models.ViewModels
{
    public class DataVisualizationViewModel
    {
        public AggregatedData AggregatedData { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public TimeframeOptions SelectedTimeframe { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}
