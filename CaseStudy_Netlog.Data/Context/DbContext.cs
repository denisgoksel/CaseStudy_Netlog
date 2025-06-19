using CaseStudy_Netlog.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy_Netlog.Data.Context
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet Properties - Veritabanındaki tablolar
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

        // Model oluşturma ve ilişkileri ayarlama
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order - OrderItem 1-N ilişkisi
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order - Delivery 1-1 ilişkisi
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Order)
                .WithOne(o => o.Delivery)
                .HasForeignKey<Delivery>(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Kolon konfigürasyonları (zorunlu alanlar, uzunluklar)
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.DeliveryPoint).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ReceiverName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ContactPhone).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired();
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Quantity).IsRequired();
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.Property(e => e.PlateNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DeliveredBy).IsRequired().HasMaxLength(255);
                entity.Property(e => e.DeliveryDate).IsRequired();
            });
        }
    }
}
