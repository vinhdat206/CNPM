using CNPMFastFood.Services;
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // =====================================================
    // Controller quản lý khách hàng trong khu vực Admin
    // URL mặc định:
    // /Admin/Customer/Index
    // =====================================================

    [Area("Admin")]
    public class CustomerController : Controller
    {
        // =====================================================
        // Khai báo service xử lý nghiệp vụ khách hàng
        // Service này sẽ chứa logic thao tác database
        // như:
        // - Lấy danh sách user
        // - Khóa tài khoản
        // - Reset password
        // - Xóa user
        // =====================================================

        private readonly ICustomerService _customerService;

        // =====================================================
        // Constructor
        // ASP.NET Core sẽ tự động Inject ICustomerService
        // thông qua Dependency Injection (DI)
        // =====================================================

        public CustomerController(
            ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // =====================================================
        // HIỂN THỊ DANH SÁCH USER
        // =====================================================

        // keyword:
        // dùng để tìm kiếm user theo tên/email/sđt...
        public async Task<IActionResult> Index(
            string keyword)
        {
            // =================================================
            // Gọi service lấy toàn bộ khách hàng
            // Nếu keyword != null
            // thì service sẽ tự lọc dữ liệu
            // =================================================

            var customers =
                await _customerService
                    .GetAllCustomersAsync(keyword);

            // =================================================
            // Lưu keyword vào ViewBag
            // để hiển thị lại giá trị trên ô search
            // sau khi submit form
            // =================================================

            ViewBag.Keyword = keyword;

            // =================================================
            // Trả dữ liệu sang View Index.cshtml
            // Model của View sẽ là danh sách customers
            // =================================================

            return View(customers);
        }

        // =====================================================
        // XEM CHI TIẾT USER
        // =====================================================

        // Action này dùng để hiển thị thông tin chi tiết
        // của một khách hàng

        // Nếu bạn đang dùng popup/modal trong Index.cshtml
        // thì action này có thể không cần dùng nữa

        public async Task<IActionResult> Detail(int id)
        {
            // =================================================
            // Tìm user theo ID
            // =================================================

            var customer =
                await _customerService
                    .GetCustomerByIdAsync(id);

            // =================================================
            // Nếu không tìm thấy user
            // trả về lỗi 404
            // =================================================

            if (customer == null)
            {
                return NotFound();
            }

            // =================================================
            // Trả dữ liệu user sang Detail.cshtml
            // =================================================

            return View(customer);
        }

        // =====================================================
        // CẬP NHẬT ROLE USER
        // =====================================================

        // Chỉ nhận request POST
        [HttpPost]

        // Kiểm tra AntiForgeryToken để chống CSRF Attack
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(
            int id,
            string role)
        {
            // =================================================
            // Gọi service cập nhật role
            // role có thể là:
            // - Admin
            // - Customer
            // =================================================

            await _customerService
                .UpdateRoleAsync(id, role);

            // =================================================
            // Sau khi cập nhật xong
            // quay lại trang danh sách
            // =================================================

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // KHÓA TÀI KHOẢN USER
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(int id)
        {
            // =================================================
            // Gọi service khóa tài khoản
            // Thường sẽ set:
            // IsBlocked = true
            // =================================================

            await _customerService
                .BlockUserAsync(id);

            // =================================================
            // Quay lại trang danh sách
            // =================================================

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // MỞ KHÓA TÀI KHOẢN USER
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(int id)
        {
            // =================================================
            // Gọi service mở khóa tài khoản
            // Thường sẽ set:
            // IsBlocked = false
            // =================================================

            await _customerService
                .UnblockUserAsync(id);

            // =================================================
            // Quay lại trang danh sách
            // =================================================

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // RESET PASSWORD USER
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
            int id,
            string newPassword)
        {
            // =================================================
            // Kiểm tra admin đã nhập password mới chưa
            // string.IsNullOrWhiteSpace:
            // kiểm tra null, rỗng hoặc toàn khoảng trắng
            // =================================================

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return RedirectToAction(nameof(Index));
            }

            // =================================================
            // Gọi service reset password
            //
            // Trong service:
            // Password sẽ được mã hóa BCrypt
            // trước khi lưu xuống database
            // =================================================

            await _customerService
                .ResetPasswordAsync(id, newPassword);

            // =================================================
            // Quay lại trang danh sách
            // =================================================

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // XÓA USER
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // =================================================
            // Gọi service xóa user khỏi database
            // =================================================

            await _customerService
                .DeleteCustomerAsync(id);

            // =================================================
            // Sau khi xóa xong
            // quay lại trang danh sách
            // =================================================

            return RedirectToAction(nameof(Index));
        }
    }
}