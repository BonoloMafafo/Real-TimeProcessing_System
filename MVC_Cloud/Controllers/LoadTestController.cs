using Microsoft.AspNetCore.Mvc;
using SharedData.Entities;
using SharedData.Services;

namespace MVC_Cloud.Controllers
{
    public class LoadTestController : Controller
    {
        private readonly QueueService _queueService;

        public LoadTestController(QueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost]
        public async Task<IActionResult> SimulateLoad(int orderCount = 100)
        {
            var tasks = new List<Task>();
            var random = new Random();

            for (int i = 0; i < orderCount; i++)
            {
                var orderEvent = new OrderEvent
                {
                    OrderId = $"ORDER-{Guid.NewGuid():N}",
                    CustomerId = $"CUST-{random.Next(1000, 9999)}",
                    Amount = (decimal)(random.NextDouble() * 2000 + 50), // ZAR amounts
                    ProductId = $"PROD-{random.Next(100, 999)}",
                    Quantity = random.Next(1, 5),
                    EventTime = DateTime.UtcNow,
                    EventType = "order_created"
                };

                tasks.Add(_queueService.SendMessageAsync(orderEvent));
            }

            await Task.WhenAll(tasks);
            return Json(new { success = true, message = $"Sent {orderCount} orders for processing" });
        }
    }
}