namespace SharedData.Entities
{
    public class ProcessedOrder
    {
        public int Id { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public DateTime ProcessedTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ProcessingResult { get; set; } = string.Empty;
    }
}