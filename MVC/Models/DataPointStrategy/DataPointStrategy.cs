using Common.Enums;
using Common.Models;

namespace MVC.Controllers
{
    public partial class DataVisualizationController
    {
        public abstract class DataPointStrategy
        {
            public abstract Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
                TimeframeOptions timeframeOptions, BearerToken? bearerToken);

            // Sorts data by timestamp
            protected virtual List<DataPoint> SortList(List<DataPoint> dataPoints)
            {
                dataPoints.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

                return dataPoints;
            }

            protected virtual List<DataPoint> NestedMultiplication(List<DataPoint> list1, List<DataPoint> list2)
            {
                List<DataPoint> dataPoints = new List<DataPoint>();

                if (list1.Count != list2.Count)
                {
                    // Implement robust error handling
                }

                for (int i = 0; i < list1.Count; i ++)
                {
                    // Remove when robust error handling is implemented
                    if (list1[i].Value == 0 || list1[i].Timestamp == DateTime.MinValue
                        || list2[i].Value == 0 || list2[i].Timestamp == DateTime.MinValue)
                    {
                        continue;
                    }

                    dataPoints.Add(new DataPoint(
                        list1[i].Timestamp,
                        list1[i].Value * list2[i].Value
                        ));
                }

                return dataPoints;
            }
        }
    }
}
