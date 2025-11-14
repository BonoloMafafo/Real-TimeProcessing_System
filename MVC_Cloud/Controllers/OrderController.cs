using Microsoft.AspNetCore.Mvc;
using MVC_Cloud.Models;
using SharedData;
using SharedData.Entities;
using SharedData.Services;

namespace MVC_Cloud.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly QueueService _queueService;

        public OrderController(AppDbContext context, QueueService queueService)
        {
            _context = context;
            _queueService = queueService;
        }

        public IActionResult Index()
        {
            var processedCount = _context.ProcessedOrders.Count();
            var totalOrderEvents = _context.OrderEvents.Count();
            
            var model = new DashboardViewModel
            {
                TotalOrders = totalOrderEvents > 0 ? totalOrderEvents : processedCount,
                TotalRevenue = _context.ProcessedOrders.Any() ? _context.ProcessedOrders.Sum(o => o.TotalAmount) : 0,
                ProcessedToday = _context.ProcessedOrders.Count(o => o.ProcessedTime.Date == DateTime.Today),
                RecentOrders = _context.ProcessedOrders.Any() ? 
                    _context.ProcessedOrders
                        .OrderByDescending(o => o.ProcessedTime)
                        .Take(10)
                        .Select(o => new ProcessedOrderSummary
                        {
                            OrderId = o.OrderId,
                            TotalAmount = o.TotalAmount,
                            ProcessedTime = o.ProcessedTime,
                            Status = o.Status
                        }).ToList() :
                    _context.OrderEvents
                        .OrderByDescending(o => o.EventTime)
                        .Take(10)
                        .Select(o => new ProcessedOrderSummary
                        {
                            OrderId = o.OrderId,
                            TotalAmount = o.Amount * o.Quantity,
                            ProcessedTime = o.EventTime,
                            Status = o.Status
                        }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel model)
        {
            try
            {
                var orderEvent = new OrderEvent
                {
                    OrderId = model.OrderId,
                    CustomerId = model.CustomerId,
                    Amount = model.Amount,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity,
                    EventTime = DateTime.UtcNow,
                    EventType = model.EventType
                };

                _context.OrderEvents.Add(orderEvent);
                await _context.SaveChangesAsync();
                await _queueService.SendMessageAsync(orderEvent);

                return Json(new { success = true, message = "Order created and sent to processing" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}