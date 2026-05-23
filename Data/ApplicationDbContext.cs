// File: Data/AppDbContext.cs
// Mô tả:
// Quản lý database SQL Server

using CNPMFastFood.Models;
using Microsoft.EntityFrameworkCore;

namespace CNPMFastFood.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =========================
        // PRODUCT
        // =========================

        public DbSet<Product> Products { get; set; }

        // =========================
        // CART
        // =========================

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        // =========================
        // ORDER
        // =========================

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        // =========================
        // USER
        // =========================

        public DbSet<User> Users { get; set; }

        // =========================
        // CONTACT MESSAGE
        // =========================

        public DbSet<ContactMessage> ContactMessages { get; set; }

        // =========================
        // REVIEW
        // =========================

        public DbSet<Review> Reviews { get; set; }

        // =========================
        // MODEL CONFIG
        // =========================

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
// DECIMAL CONFIG
// =========================
// Fix warning decimal precision cho SQL Server

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CartItem>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(o => o.Price)
                .HasPrecision(18, 2);
            // =========================
            // CART - CART ITEM
            // =========================

            modelBuilder.Entity<CartItem>()
                .HasOne<Cart>()
                .WithMany(c => c.Items)
                .HasForeignKey("CartId");

            // =========================
            // ORDER - ORDER DETAIL
            // =========================

            modelBuilder.Entity<OrderDetail>()
                .HasOne(o => o.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(o => o.OrderId)

                // FIX SQL SERVER
                // Tránh lỗi multiple cascade paths
                .OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<Setting> Settings { get; set; }
    }
}