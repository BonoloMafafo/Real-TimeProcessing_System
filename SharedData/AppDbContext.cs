using Microsoft.EntityFrameworkCore;
using SharedData.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SharedData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<OrderEvent> OrderEvents => Set<OrderEvent>();
        public DbSet<ProcessedOrder> ProcessedOrders => Set<ProcessedOrder>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProcessingMetrics> ProcessingMetrics => Set<ProcessingMetrics>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<OrderEvent>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.OrderId);
            });
            mb.Entity<ProcessedOrder>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.OrderId);
            });
            mb.Entity<Customer>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.CustomerId);
            });
            mb.Entity<Product>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.ProductId);
            });
            mb.Entity<ProcessingMetrics>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.Timestamp);
            });
        }

    }
}
