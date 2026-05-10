// File: Services/OrderService.cs
// Mô tả:
// Xử lý logic đặt hàng

using CNPMFastFood.Data;
using CNPMFastFood.Models;

namespace CNPMFastFood.Services
{
    public class OrderService
    {
        // Kết nối database
        private readonly AppDbContext _context;

        // Constructor
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // CREATE ORDER
        // =========================

        // order:
        // thông tin khách hàng

        // cart:
        // danh sách sản phẩm trong giỏ hàng

        public void CreateOrder(
            Order order,
            List<CartItem> cart)
        {
            // =========================
            // THÔNG TIN ĐƠN HÀNG
            // =========================

            // lấy thời gian hiện tại
            order.OrderDate = DateTime.Now;

            // trạng thái mặc định
            order.Status = "Pending";

            // tính tổng tiền
            order.TotalAmount =
                cart.Sum(x =>
                    x.Price * x.Quantity);

            // =========================
            // LƯU ORDER
            // =========================

            _context.Orders.Add(order);

            // save để lấy Order.Id
            _context.SaveChanges();

            // =========================
            // LƯU ORDER DETAIL
            // =========================

            foreach (var item in cart)
            {
                // tạo detail mới
                var detail = new OrderDetail
                {
                    // Id đơn hàng vừa tạo
                    OrderId = order.Id,

                    // Id sản phẩm
                    ProductId = item.Id,

                    // Tên sản phẩm
                    ProductName = item.Name,

                    // Giá sản phẩm
                    Price = item.Price,

                    // Số lượng
                    Quantity = item.Quantity
                };

                // add vào database
                _context.OrderDetails.Add(detail);
            }

            // lưu toàn bộ detail
            _context.SaveChanges();
        }
    }
}