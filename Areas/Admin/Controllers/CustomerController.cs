using CNPMFastFood.Services;
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // Controller thuộc khu vực Admin
    [Area("Admin")]
    public class CustomerController : Controller
    {
        // Service xử lý nghiệp vụ khách hàng
        private readonly ICustomerService _customerService;

        // Inject ICustomerService vào Controller
        public CustomerController(
            ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // =========================
        // HIỂN THỊ DANH SÁCH USER
        // =========================

        public async Task<IActionResult> Index(
            string keyword)
        {
            // Gọi service lấy danh sách user
            // Nếu keyword có giá trị thì service sẽ tìm kiếm
            var customers =
                await _customerService
                    .GetAllCustomersAsync(keyword);

            // Lưu keyword vào ViewBag
            // để hiển thị lại trên ô tìm kiếm
            ViewBag.Keyword = keyword;

            // Trả danh sách user sang View Index.cshtml
            return View(customers);
        }

        // =========================
        // CHI TIẾT USER
        // =========================
        // Hiện tại nếu bạn dùng popup trong Index.cshtml
        // thì action Detail này có thể không cần dùng nữa

        public async Task<IActionResult> Detail(int id)
        {
            // Lấy thông tin user theo Id
            var customer =
                await _customerService
                    .GetCustomerByIdAsync(id);

            // Nếu không tìm thấy user
            if (customer == null)
            {
                return NotFound();
            }

            // Trả user sang view Detail.cshtml
            return View(customer);
        }

        // =========================
        // ĐỔI ROLE USER
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(
            int id,
            string role)
        {
            // Cập nhật quyền user/admin
            await _customerService
                .UpdateRoleAsync(id, role);

            // Quay lại trang danh sách
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // KHÓA TÀI KHOẢN
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(int id)
        {
            // Cập nhật IsBlocked = true
            await _customerService
                .BlockUserAsync(id);

            // Quay lại trang danh sách
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // MỞ KHÓA TÀI KHOẢN
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(int id)
        {
            // Cập nhật IsBlocked = false
            await _customerService
                .UnblockUserAsync(id);

            // Quay lại trang danh sách
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // RESET PASSWORD
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
            int id,
            string newPassword)
        {
            // Nếu admin chưa nhập mật khẩu mới
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return RedirectToAction(nameof(Index));
            }

            // Reset mật khẩu mới
            // Service sẽ mã hóa BCrypt trước khi lưu DB
            await _customerService
                .ResetPasswordAsync(id, newPassword);

            // Quay lại trang danh sách
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // XÓA USER
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Xóa user khỏi database
            await _customerService
                .DeleteCustomerAsync(id);

            // Quay lại trang danh sách
            return RedirectToAction(nameof(Index));
        }
    }
}