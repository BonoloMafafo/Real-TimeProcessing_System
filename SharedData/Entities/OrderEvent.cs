namespace SharedData.Entities
{
    public class OrderEvent
    {
        public int Id { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime EventTime { get; set; }
        public string EventType { get; set; } = string.Empty; // "order_created", "order_updated", "order_cancelled"
        public string Status { get; set; } = "pending";
    }
}