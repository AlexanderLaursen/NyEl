using Common.Enums;
using Common.Models;

namespace MVC.Controllers
{
    public partial class DataVisualizationController
    {
        public abstract class DataPointStrategy
        {
            public abstract Task<List<DataPoint>> GetDataPoints(DateTime dateTime,
                TimeframeOptions timeframeOptions, string? bearerToken = default);

            protected virtual List<DataPoint> SortList(List<DataPoint> dataPoints)
            {
                dataPoints.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

                return dataPoints;
            }

            protected virtual List<DataPoint> MatrixMultiplication(List<DataPoint> list1, List<DataPoint> list2)
            {
                List<DataPoint> dataPoints = new List<DataPoint>();

                if (list1.Count != list2.Count)
                {
                    throw new Exception();
                }

                for (int i = 0; i < list1.Count; i ++)
                {
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
