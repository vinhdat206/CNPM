using CNPMFastFood.Data; // Chứa AppDbContext để thao tác với database
using CNPMFastFood.Models; // Chứa model Review, Order,...
using Microsoft.AspNetCore.Authorization; // Dùng [Authorize] để yêu cầu đăng nhập
using Microsoft.AspNetCore.Mvc; // Dùng cho Controller, IActionResult, RedirectToAction,...
using Microsoft.EntityFrameworkCore; // Dùng Include để load dữ liệu liên quan

namespace CNPMFastFood.Controllers
{
    // Yêu cầu người dùng phải đăng nhập mới được đánh giá
    [Authorize]
    public class ReviewController : Controller
    {
        // DbContext dùng để truy cập database
        private readonly AppDbContext _context;

        // Constructor Dependency Injection
        public ReviewController(AppDbContext context)
        {
            // Gán DbContext được inject vào biến _context
            _context = context;
        }

        // =========================
        // CREATE REVIEW
        // Tạo đánh giá cho đơn hàng
        // =========================

        [HttpPost]

        // Chống tấn công CSRF khi gửi form
        [ValidateAntiForgeryToken]
        public IActionResult Create(int orderId, int rating, string comment)
        {
            // Kiểm tra số sao phải nằm trong khoảng từ 1 đến 5
            if (rating < 1 || rating > 5)
            {
                // Lưu thông báo lỗi tạm thời
                TempData["Error"] = "Số sao không hợp lệ!";

                // Quay lại trang lịch sử đơn hàng
                return RedirectToAction("History", "Order");
            }

            // Kiểm tra nội dung đánh giá không được rỗng
            if (string.IsNullOrWhiteSpace(comment))
            {
                // Lưu thông báo lỗi
                TempData["Error"] = "Vui lòng nhập nội dung đánh giá!";

                // Quay lại trang lịch sử đơn hàng
                return RedirectToAction("History", "Order");
            }

            // Lấy tên người dùng đang đăng nhập
            // Nếu không lấy được thì dùng mặc định là "Khách hàng"
            var userName = User.Identity?.Name ?? "Khách hàng";

            // Tìm đơn hàng theo orderId
            // Include OrderDetails để lấy danh sách sản phẩm trong đơn hàng
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.Id == orderId);

            // Kiểm tra đơn hàng có tồn tại và đã hoàn thành chưa
            if (order == null || order.Status != "Completed")
            {
                // Chỉ cho phép đánh giá đơn hàng đã hoàn thành
                TempData["Error"] = "Chỉ có thể đánh giá đơn hàng đã hoàn thành!";

                // Quay lại trang lịch sử đơn hàng
                return RedirectToAction("History", "Order");
            }

            // Duyệt từng sản phẩm trong đơn hàng
            foreach (var item in order.OrderDetails)
            {
                // Tạo đánh giá cho từng sản phẩm thuộc đơn hàng
                var review = new Review
                {
                    // Tên người đánh giá
                    UserName = userName,

                    // Số sao đánh giá
                    Rating = rating,

                    // Nội dung bình luận
                    Comment = comment,

                    // Id sản phẩm được đánh giá
                    ProductId = item.ProductId,

                    // Id đơn hàng liên quan
                    OrderId = orderId,

                    // Thời gian tạo đánh giá
                    CreatedAt = DateTime.Now
                };

                // Thêm review vào DbSet Reviews
                _context.Reviews.Add(review);
            }

            // Lưu tất cả đánh giá xuống database
            _context.SaveChanges();

            // Thông báo thành công
            TempData["Success"] = "Gửi đánh giá thành công!";

            // Quay lại trang lịch sử đơn hàng
            return RedirectToAction("History", "Order");
        }
    }
}