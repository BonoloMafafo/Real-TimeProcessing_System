using Microsoft.AspNetCore.Mvc;
using SharedData;
using SharedData.Entities;
using SharedData.Services;
using System.Text.Json;

namespace MVC_Cloud.Controllers
{
    public class ProcessorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly QueueService _queueService;

        public ProcessorController(AppDbContext context, QueueService queueService)
        {
            _context = context;
            _queueService = queueService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPendingOrders()
        {
            var pendingOrders = _context.OrderEvents
                .Where(o => o.Status == "pending")
                .ToList();

            foreach (var orderEvent in pendingOrders)
            {
                var processedOrder = new ProcessedOrder
                {
                    OrderId = orderEvent.OrderId,
                    CustomerId = orderEvent.CustomerId,
                    TotalAmount = orderEvent.Amount * orderEvent.Quantity,
                    TotalItems = orderEvent.Quantity,
                    ProcessedTime = DateTime.UtcNow,
                    Status = "processed",
                    ProcessingResult = "Successfully processed order"
                };

                _context.ProcessedOrders.Add(processedOrder);
                orderEvent.Status = "processed";
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, processed = pendingOrders.Count });
        }
    }
}