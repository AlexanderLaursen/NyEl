using Common.Models;

namespace MVC.Helpers
{
    public static class AggregationHelper
    {
        public static decimal Sum(List<DataPoint> dataPoints)
        {
            if (dataPoints == null || dataPoints.Count == 0)
            {
                return 0;
            }
            return dataPoints.Sum(dp => dp.Value);
        }

        public static decimal Average(List<DataPoint> dataPoints)
        {
            if (dataPoints == null || dataPoints.Count == 0)
            {
                return 0;
            }
            return dataPoints.Average(dp => dp.Value);
        }

        public static decimal Min(List<DataPoint> dataPoints)
        {
            if (dataPoints == null || dataPoints.Count == 0)
            {
                return 0;
            }
            return dataPoints.Min(dp => dp.Value);
        }

        public static decimal Max(List<DataPoint> dataPoints)
        {
            if (dataPoints == null || dataPoints.Count == 0)
            {
                return 0;
            }
            return dataPoints.Max(dp => dp.Value);
        }
    }
}
