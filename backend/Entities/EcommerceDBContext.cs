using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using System;

namespace backend.Entities
{
    public class EcommerceDBContext : DbContext
    {
        public EcommerceDBContext(DbContextOptions<EcommerceDBContext> options) : base(options)
        {
        }

        // DbSets for each entity
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<PurchaseReceipt> PurchaseReceipt { get; set; }
        public DbSet<PurchaseReceiptDetail> PurchaseReceiptDetail { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public DbSet<GoodsInspectionItem> GoodsInspectionItems { get; set; }
        public DbSet<GoodsInspection> GoodsInspections { get; set; }
        public DbSet<StockHistory> StockHistories { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<WishlistedItem> WishlistedItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoodsInspection>()
                .HasOne(g => g.CreatedByUser)
                .WithMany(u => u.GoodsInspections)
                .HasForeignKey(g => g.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GoodsInspection>()
                .HasOne(g => g.InchargePerson)
                .WithMany()
                .HasForeignKey(g => g.InchargePersonId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
