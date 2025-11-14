using Microsoft.AspNetCore.Mvc;
using SharedData;

namespace MVC_Cloud.Controllers
{
    public class DebugController : Controller
    {
        private readonly AppDbContext _context;

        public DebugController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult CheckData()
        {
            var orderEventsCount = _context.OrderEvents.Count();
            var processedOrdersCount = _context.ProcessedOrders.Count();
            var totalRevenue = _context.ProcessedOrders.Any() ? _context.ProcessedOrders.Sum(o => o.TotalAmount) : 0;

            return Json(new 
            { 
                orderEvents = orderEventsCount,
                processedOrders = processedOrdersCount,
                totalRevenue = totalRevenue,
                recentOrderEvents = _context.OrderEvents.Take(5).Select(o => new { o.OrderId, o.Amount, o.Status }).ToList(),
                recentProcessedOrders = _context.ProcessedOrders.Take(5).Select(o => new { o.OrderId, o.TotalAmount, o.Status }).ToList()
            });
        }
    }
}