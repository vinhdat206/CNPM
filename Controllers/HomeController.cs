// File: Controllers/HomeController.cs
// Mô tả:
// Controller xử lý:
// - Trang chủ
// - Trang giới thiệu
// - Trang liên hệ
// - Chính sách website

using Microsoft.AspNetCore.Mvc; // Dùng cho MVC Controller
using CNPMFastFood.Services; // Chứa ProductService
using CNPMFastFood.Models; // Chứa các model
using CNPMFastFood.Data; // Chứa AppDbContext kết nối database

namespace CNPMFastFood.Controllers
{
    // Controller quản lý các trang công khai của website
    public class HomeController : Controller
    {
        // Service xử lý sản phẩm
        private readonly ProductService _productService;

        // DbContext thao tác database
        private readonly AppDbContext _context;

        // Constructor Dependency Injection
        public HomeController(
            ProductService productService,
            AppDbContext context)
        {
            // Gán ProductService
            _productService = productService;

            // Gán DbContext
            _context = context;
        }

        // =========================
        // HOME PAGE
        // Trang chủ
        // =========================

        public IActionResult Index()
        {
            // Lấy toàn bộ sản phẩm
            var products = _productService.GetAll()

                // Chỉ lấy sản phẩm nổi bật
                .Where(p => p.Featured == true)

                // Sắp xếp mới nhất trước
                .OrderByDescending(p => p.Id)

                // Chuyển sang List
                .ToList();

            // Truyền dữ liệu sang View
            return View(products);
        }

        // =========================
        // ABOUT PAGE
        // Trang giới thiệu
        // =========================

        public IActionResult About()
        {
            // Trả về View About.cshtml
            return View();
        }

        // =========================
        // CONTACT PAGE - GET
        // Hiển thị form liên hệ
        // =========================

        [HttpGet]
        public IActionResult Contact()
        {
            // Tạo model rỗng cho form
            return View(new ContactMessage());
        }

        // =========================
        // CONTACT PAGE - POST
        // Nhận dữ liệu user gửi
        // Lưu vào database
        // =========================

        [HttpPost]

        // Chống tấn công CSRF
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMessage model)
        {
            // Kiểm tra dữ liệu hợp lệ
            if (!ModelState.IsValid)
            {
                // Nếu lỗi -> trả lại form
                return View(model);
            }

            // Gán thời gian gửi liên hệ
            model.CreatedAt = DateTime.Now;

            // Mặc định chưa đọc
            model.IsRead = false;

            // Thêm vào bảng ContactMessages
            _context.ContactMessages.Add(model);

            // Lưu thay đổi xuống database
            await _context.SaveChangesAsync();

            // Thông báo thành công
            ViewBag.Success = "Gửi liên hệ thành công!";

            // Xóa trạng thái ModelState cũ
            ModelState.Clear();

            // Trả về form mới rỗng
            return View(new ContactMessage());
        }

        // =========================
        // POLICY PAGE
        // Trang chính sách
        // =========================

        public IActionResult Policy()
        {
            // Trả về View Policy.cshtml
            return View();
        }
    }
}