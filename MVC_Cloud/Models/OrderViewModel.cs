namespace MVC_Cloud.Models
{
    public class OrderViewModel
    {
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string EventType { get; set; } = "order_created";
    }

    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ProcessedToday { get; set; }
        public List<ProcessedOrderSummary> RecentOrders { get; set; } = new();
    }

    public class ProcessedOrderSummary
    {
        public string OrderId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime ProcessedTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}