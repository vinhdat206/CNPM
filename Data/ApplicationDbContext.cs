// File: Data/AppDbContext.cs
// Mô tả:
// Quản lý database SQL Server bằng Entity Framework Core

using CNPMFastFood.Models; // Chứa các model/entity như Product, Cart, Order, User,...
using Microsoft.EntityFrameworkCore; // Thư viện Entity Framework Core

namespace CNPMFastFood.Data
{
    // AppDbContext là lớp đại diện cho database của ứng dụng
    // Kế thừa DbContext để làm việc với Entity Framework Core
    public class AppDbContext : DbContext
    {
        // Constructor nhận cấu hình DbContext từ Program.cs / Startup.cs
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options) // Truyền options cho DbContext cha
        {
        }

        // =========================
        // PRODUCT
        // Bảng sản phẩm
        // =========================

        // Đại diện cho bảng Products trong database
        public DbSet<Product> Products { get; set; }

        // =========================
        // CART
        // Bảng giỏ hàng
        // =========================

        // Đại diện cho bảng Carts
        public DbSet<Cart> Carts { get; set; }

        // Đại diện cho bảng CartItems, lưu từng sản phẩm trong giỏ
        public DbSet<CartItem> CartItems { get; set; }

        // =========================
        // ORDER
        // Bảng đơn hàng
        // =========================

        // Đại diện cho bảng Orders
        public DbSet<Order> Orders { get; set; }

        // Đại diện cho bảng OrderDetails, lưu chi tiết sản phẩm trong đơn hàng
        public DbSet<OrderDetail> OrderDetails { get; set; }

        // =========================
        // USER
        // Bảng người dùng
        // =========================

        // Đại diện cho bảng Users
        public DbSet<User> Users { get; set; }

        // =========================
        // CONTACT MESSAGE
        // Bảng tin nhắn liên hệ
        // =========================

        // Đại diện cho bảng ContactMessages
        public DbSet<ContactMessage> ContactMessages { get; set; }

        // =========================
        // REVIEW
        // Bảng đánh giá sản phẩm
        // =========================

        // Đại diện cho bảng Reviews
        public DbSet<Review> Reviews { get; set; }

        // =========================
        // SETTING
        // Bảng cấu hình hệ thống
        // =========================

        // Đại diện cho bảng Settings
        public DbSet<Setting> Settings { get; set; }

        // =========================
        // MODEL CONFIG
        // Cấu hình quan hệ và kiểu dữ liệu cho database
        // =========================

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            // Gọi cấu hình mặc định của Entity Framework Core
            base.OnModelCreating(modelBuilder);

            // =========================
            // DECIMAL CONFIG
            // Cấu hình độ chính xác cho kiểu decimal
            // Fix warning decimal precision cho SQL Server
            // =========================

            // Cấu hình Price của Product có tối đa 18 chữ số, 2 chữ số sau dấu phẩy
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // Cấu hình Price của CartItem
            modelBuilder.Entity<CartItem>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            // Cấu hình TotalAmount của Order
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            // Cấu hình Price của OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .Property(o => o.Price)
                .HasPrecision(18, 2);

            // =========================
            // CART - CART ITEM
            // Quan hệ 1 giỏ hàng có nhiều sản phẩm trong giỏ
            // =========================

            modelBuilder.Entity<CartItem>()

                // Mỗi CartItem thuộc về một Cart
                .HasOne<Cart>()

                // Một Cart có nhiều CartItem thông qua thuộc tính Items
                .WithMany(c => c.Items)

                // Khóa ngoại là CartId
                .HasForeignKey("CartId");

            // =========================
            // ORDER - ORDER DETAIL
            // Quan hệ 1 đơn hàng có nhiều chi tiết đơn hàng
            // =========================

            modelBuilder.Entity<OrderDetail>()

                // Mỗi OrderDetail thuộc về một Order
                .HasOne(o => o.Order)

                // Một Order có nhiều OrderDetail
                .WithMany(o => o.OrderDetails)

                // Khóa ngoại OrderId
                .HasForeignKey(o => o.OrderId)

                // Không tự động xóa dây chuyền
                // Tránh lỗi multiple cascade paths trong SQL Server
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}