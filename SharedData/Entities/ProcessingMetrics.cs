namespace SharedData.Entities
{
    public class ProcessingMetrics
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int OrdersProcessed { get; set; }
        public decimal AverageProcessingTime { get; set; }
        public int ErrorCount { get; set; }
        public decimal ThroughputPerSecond { get; set; }
    }
}