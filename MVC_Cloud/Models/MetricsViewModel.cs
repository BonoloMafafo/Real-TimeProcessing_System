namespace MVC_Cloud.Models
{
    public class MetricsViewModel
    {
        public int OrdersPerSecond { get; set; }
        public decimal AverageProcessingTime { get; set; }
        public int ActiveConnections { get; set; }
        public decimal SystemLoad { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}