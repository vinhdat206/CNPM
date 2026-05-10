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

        public DbSet<Product> Products
        {
            get; set;
        }

        // =========================
        // CART
        // =========================

        public DbSet<Cart> Carts
        {
            get; set;
        }

        public DbSet<CartItem> CartItems
        {
            get; set;
        }

        // =========================
        // ORDER
        // =========================

        public DbSet<Order> Orders
        {
            get; set;
        }

        public DbSet<OrderDetail> OrderDetails
        {
            get; set;
        }

        // =========================
        // USER
        // =========================

        public DbSet<User> Users
        {
            get; set;
        }

        // =========================
        // MODEL CONFIG
        // =========================

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                // tránh lỗi multiple cascade paths

                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}