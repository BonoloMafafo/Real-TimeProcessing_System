using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SharedData;
using SharedData.Entities;
using System.Text.Json;

namespace Injector
{
    public class OrderProcessorFunction
    {
        private readonly ILogger<OrderProcessorFunction> _logger;
        private readonly AppDbContext _context;

        public OrderProcessorFunction(ILogger<OrderProcessorFunction> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("ProcessOrderQueue")]
        public async Task ProcessOrderQueue(
            [QueueTrigger("order-events", Connection = "StorageConnection")] string queueMessage)
        {
            try
            {
                var orderEvent = JsonSerializer.Deserialize<OrderEvent>(queueMessage);
                if (orderEvent != null)
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
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation($"Processed order {orderEvent.OrderId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing order queue message: {QueueMessage}", queueMessage);
            }
        }
    }
}
