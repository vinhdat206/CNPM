// Import service xử lý nghiệp vụ đơn hàng
using CNPMFastFood.Services;

// Import thư viện MVC để dùng Controller, IActionResult, ViewData, TempData...
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // Controller này thuộc khu vực Admin
    // URL thường có dạng: /Admin/Order/Index
    [Area("Admin")]
    public class OrderController : Controller
    {
        // Khai báo service dùng để xử lý các chức năng liên quan đến đơn hàng
        private readonly OrderService _orderService;

        // Constructor
        // ASP.NET Core sẽ tự động inject OrderService vào Controller
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // =========================
        // HIỂN THỊ DANH SÁCH ĐƠN HÀNG
        // =========================

        // GET: /Admin/Order
        public async Task<IActionResult> Index()
        {
            // Tiêu đề trang
            ViewData["Title"] = "Đơn hàng";

            // Tiêu đề chính hiển thị trong View
            ViewData["PageTitle"] = "Quản lý đơn hàng";

            // Gọi service lấy toàn bộ danh sách đơn hàng
            var orders = await _orderService.GetAllOrdersAsync();

            // Trả danh sách đơn hàng sang View Index.cshtml
            return View(orders);
        }

        // =========================
        // XEM CHI TIẾT ĐƠN HÀNG
        // =========================

        // GET: /Admin/Order/Details/5
        // id là mã đơn hàng cần xem chi tiết
        public async Task<IActionResult> Details(int id)
        {
            // Tiêu đề trang
            ViewData["Title"] = "Chi tiết đơn hàng";

            // Tiêu đề chính hiển thị trong View
            ViewData["PageTitle"] = "Chi tiết đơn hàng";

            // Gọi service lấy thông tin đơn hàng theo id
            var order = await _orderService.GetOrderByIdAsync(id);

            // Nếu không tìm thấy đơn hàng
            if (order == null)
            {
                // Lưu thông báo lỗi tạm thời
                // TempData vẫn còn dữ liệu sau khi Redirect
                TempData["Error"] = "Không tìm thấy đơn hàng!";

                // Quay lại trang danh sách đơn hàng
                return RedirectToAction(nameof(Index));
            }

            // Trả thông tin đơn hàng sang View Details.cshtml
            return View(order);
        }

        // =========================
        // CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG
        // =========================

        // POST: /Admin/Order/UpdateStatus
        [HttpPost]

        // Chống tấn công CSRF
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            // Kiểm tra trạng thái có rỗng/null/toàn khoảng trắng không
            if (string.IsNullOrWhiteSpace(status))
            {
                // Nếu không hợp lệ thì lưu thông báo lỗi
                TempData["Error"] = "Trạng thái không hợp lệ!";

                // Quay lại trang danh sách
                return RedirectToAction(nameof(Index));
            }

            // Gọi service cập nhật trạng thái đơn hàng
            await _orderService.UpdateStatusAsync(id, status);

            // Lưu thông báo thành công
            TempData["Success"] = "Cập nhật trạng thái đơn hàng thành công!";

            // Quay lại trang danh sách đơn hàng
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // HỦY ĐƠN HÀNG
        // =========================

        // POST: /Admin/Order/Cancel
        [HttpPost]

        // Chống tấn công CSRF
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, string cancelReason)
        {
            // Kiểm tra lý do hủy có được nhập hay không
            if (string.IsNullOrWhiteSpace(cancelReason))
            {
                // Nếu chưa nhập lý do thì báo lỗi
                TempData["Error"] = "Vui lòng nhập lý do hủy đơn!";

                // Quay lại danh sách đơn hàng
                return RedirectToAction(nameof(Index));
            }

            // Gọi service hủy đơn hàng
            // Service có thể cập nhật status = Cancelled
            // và lưu lý do hủy vào database
            await _orderService.CancelOrderAsync(id, cancelReason);

            // Lưu thông báo thành công
            TempData["Success"] = "Đã hủy đơn hàng!";

            // Quay lại trang danh sách
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // XÓA ĐƠN HÀNG
        // =========================

        // POST: /Admin/Order/Delete
        [HttpPost]

        // Chống tấn công CSRF
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Gọi service xóa đơn hàng khỏi database
            await _orderService.DeleteOrderAsync(id);

            // Lưu thông báo thành công
            TempData["Success"] = "Đã xóa đơn hàng!";

            // Quay lại trang danh sách đơn hàng
            return RedirectToAction(nameof(Index));
        }
    }
}