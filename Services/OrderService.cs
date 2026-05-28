using CNPMFastFood.Data;
using CNPMFastFood.Models;
using Microsoft.EntityFrameworkCore;

namespace CNPMFastFood.Services
{
    // =========================================================
    // ORDER SERVICE
    // ---------------------------------------------------------
    // Service này xử lý các nghiệp vụ liên quan đến đơn hàng:
    // - Tạo đơn hàng
    // - Xem lịch sử đơn hàng của khách hàng
    // - Xem tất cả đơn hàng cho admin
    // - Xem chi tiết đơn hàng
    // - Cập nhật trạng thái đơn hàng
    // - Hủy đơn hàng
    // - Xóa đơn hàng
    // =========================================================
    public class OrderService
    {
        // AppDbContext dùng để làm việc với database
        private readonly AppDbContext _context;

        // Constructor nhận AppDbContext thông qua Dependency Injection
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // TẠO ĐƠN HÀNG
        // =====================================================
        public void CreateOrder(
            Order order,
            List<CartItem> cart,
            int userId,
            decimal shippingFee)
        {
            // Gán ngày đặt hàng là thời điểm hiện tại
            order.OrderDate = DateTime.Now;

            // Khi mới tạo, đơn hàng có trạng thái Pending
            order.Status = "Pending";

            // Gán đơn hàng này cho user đang đăng nhập
            order.UserId = userId;

            // Tính tổng tiền sản phẩm
            // Công thức: Giá * Số lượng
            order.SubTotal = cart.Sum(x => x.Price * x.Quantity);

            // Gán phí vận chuyển
            order.ShippingFee = shippingFee;

            // Tổng tiền = tiền sản phẩm + phí vận chuyển
            order.TotalAmount = order.SubTotal + order.ShippingFee;

            // Thêm đơn hàng vào bảng Orders
            _context.Orders.Add(order);

            // Lưu trước để sinh ra order.Id
            _context.SaveChanges();

            // Duyệt từng sản phẩm trong giỏ hàng
            // Duyệt từng sản phẩm trong giỏ hàng
            foreach (var item in cart)
            {
                // Tìm sản phẩm trong database
                var product = _context.Products
                    .FirstOrDefault(p => p.Id == item.ProductId);

                // Nếu không có sản phẩm thì bỏ qua
                if (product == null)
                    continue;

                // Kiểm tra tồn kho
                if (product.Stock < item.Quantity)
                {
                    throw new Exception(
                        $"{product.Name} không đủ số lượng tồn kho");
                }

                // Trừ tồn kho
                product.Stock -= item.Quantity;

                // Tạo chi tiết đơn hàng
                var detail = new OrderDetail
                {
                    OrderId = order.Id,

                    ProductId = item.ProductId,

                    ProductName = item.Name,

                    Price = item.Price,

                    Quantity = item.Quantity,

                    ImageUrl = item.ImageUrl
                };

                // Thêm vào database
                _context.OrderDetails.Add(detail);
            }

            // Lưu toàn bộ chi tiết đơn hàng xuống database
            _context.SaveChanges();
        }

        // =====================================================
        // LẤY LỊCH SỬ ĐƠN HÀNG CỦA USER
        // =====================================================
        public async Task<List<Order>> GetOrderHistory(int userId)
        {
            return await _context.Orders

                // Include để lấy kèm danh sách sản phẩm trong đơn hàng
                .Include(o => o.OrderDetails)

                // Chỉ lấy đơn hàng của user hiện tại
                .Where(o => o.UserId == userId)

                // Đơn mới nhất hiển thị trước
                .OrderByDescending(o => o.OrderDate)

                // Chuyển kết quả thành List
                .ToListAsync();
        }

        // =====================================================
        // LẤY TẤT CẢ ĐƠN HÀNG
        // =====================================================
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders

                // Lấy kèm chi tiết đơn hàng
                .Include(o => o.OrderDetails)

                // Sắp xếp đơn mới nhất lên đầu
                .OrderByDescending(o => o.OrderDate)

                .ToListAsync();
        }

        // =====================================================
        // LẤY CHI TIẾT ĐƠN HÀNG THEO ID
        // =====================================================
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders

                // Lấy kèm các sản phẩm trong đơn hàng
                .Include(o => o.OrderDetails)

                // Tìm đơn hàng theo Id
                // Nếu không có thì trả về null
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // =====================================================
        // CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG
        // =====================================================
        public async Task UpdateStatusAsync(int id, string status)
        {
            // Tìm đơn hàng theo Id
            var order = await _context.Orders.FindAsync(id);

            // Nếu không tồn tại thì kết thúc hàm
            if (order == null)
                return;

            // Cập nhật trạng thái mới
            order.Status = status;

            // Lưu thay đổi xuống database
            await _context.SaveChangesAsync();
        }

        // =====================================================
        // HỦY ĐƠN HÀNG
        // =====================================================
        public async Task CancelOrderAsync(int id, string cancelReason)
        {
            // Tìm đơn hàng theo Id
            var order = await _context.Orders.FindAsync(id);

            // Nếu không tồn tại thì kết thúc hàm
            if (order == null)
                return;

            // Cập nhật trạng thái đơn hàng thành Cancelled
            order.Status = "Cancelled";

            // Lưu lý do hủy đơn
            order.CancelReason = cancelReason;

            // Lưu thay đổi xuống database
            await _context.SaveChangesAsync();
        }

        // =====================================================
        // XÓA ĐƠN HÀNG
        // =====================================================
        public async Task DeleteOrderAsync(int id)
        {
            // Tìm đơn hàng và lấy kèm chi tiết đơn hàng
            var order = await _context.Orders

                .Include(o => o.OrderDetails)

                .FirstOrDefaultAsync(o => o.Id == id);

            // Nếu không tìm thấy thì kết thúc hàm
            if (order == null)
                return;

            // Xóa toàn bộ chi tiết đơn hàng trước
            // để tránh lỗi khóa ngoại
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // Xóa đơn hàng chính
            _context.Orders.Remove(order);

            // Lưu thay đổi xuống database
            await _context.SaveChangesAsync();
        }
    }
}