namespace MVC.Models
{
    public struct GraphDataPoint (string label, decimal data)
    {
        public string Label { get; set; } = label;
        public decimal Data { get; set; } = data;
    }
}
