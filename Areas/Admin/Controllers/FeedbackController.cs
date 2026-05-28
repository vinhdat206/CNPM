// Import DbContext để thao tác với database
using CNPMFastFood.Data;

// Import thư viện MVC
using Microsoft.AspNetCore.Mvc;

// Import Entity Framework Core để dùng Include, ToListAsync...
using Microsoft.EntityFrameworkCore;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // Controller thuộc khu vực Admin
    // URL: /Admin/Feedback/Index
    [Area("Admin")]
    public class FeedbackController : Controller
    {
        // Biến dùng để truy cập database
        private readonly AppDbContext _context;

        // Constructor inject AppDbContext
        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // HIỂN THỊ DANH SÁCH FEEDBACK
        // =========================

        public async Task<IActionResult> Index()
        {
            // Tiêu đề trang
            ViewData["Title"] = "Feedback";

            // Tiêu đề chính hiển thị trong View
            ViewData["PageTitle"] = "Quản lý feedback";

            // Lấy danh sách sản phẩm kèm feedback/review
            var products = await _context.Products

                // Include(p => p.Reviews):
                // lấy luôn danh sách review của từng sản phẩm
                // nếu không Include thì Reviews có thể chưa được load
                .Include(p => p.Reviews)

                // Sắp xếp sản phẩm theo tên tăng dần A-Z
                .OrderBy(p => p.Name)

                // Thực thi query và chuyển kết quả thành List
                .ToListAsync();

            // Trả danh sách sản phẩm sang View
            return View(products);
        }

        // =========================
        // XÓA FEEDBACK
        // =========================

        // Chỉ cho phép xóa bằng phương thức POST
        [HttpPost]

        // Chống tấn công CSRF
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Tìm feedback/review theo ID
            var review = await _context.Reviews.FindAsync(id);

            // Nếu không tìm thấy feedback
            if (review == null)
            {
                // Lưu thông báo lỗi tạm thời
                // TempData dùng để truyền dữ liệu sau Redirect
                TempData["Error"] = "Không tìm thấy feedback!";

                // Quay lại trang danh sách feedback
                return RedirectToAction(nameof(Index));
            }

            // Xóa feedback khỏi DbSet Reviews
            _context.Reviews.Remove(review);

            // Lưu thay đổi xuống database
            await _context.SaveChangesAsync();

            // Lưu thông báo thành công
            TempData["Success"] = "Đã xóa feedback!";

            // Quay lại trang danh sách feedback
            return RedirectToAction(nameof(Index));
        }
    }
}