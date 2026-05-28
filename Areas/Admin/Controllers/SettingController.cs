// Import model Setting
using CNPMFastFood.Models;

// Import service xử lý cài đặt website
using CNPMFastFood.Services;

// Import thư viện phân quyền
using Microsoft.AspNetCore.Authorization;

// Import thư viện MVC
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // Controller thuộc khu vực Admin
    [Area("Admin")]

    // Chỉ tài khoản có role "admin" mới được truy cập
    [Authorize(Roles = "admin")]
    public class SettingController : Controller
    {
        // Service xử lý việc lấy và lưu thông tin cài đặt
        private readonly SettingService _settingService;

        // IWebHostEnvironment dùng để lấy thông tin môi trường chạy web
        // Ví dụ: đường dẫn tới thư mục wwwroot
        private readonly IWebHostEnvironment _environment;

        // Constructor
        // ASP.NET Core sẽ tự động inject SettingService và IWebHostEnvironment
        public SettingController(
            SettingService settingService,
            IWebHostEnvironment environment)
        {
            _settingService = settingService;
            _environment = environment;
        }

        // =========================
        // HIỂN THỊ TRANG CÀI ĐẶT
        // =========================

        public IActionResult Index()
        {
            // Lấy thông tin setting hiện tại từ database
            var setting = _settingService.GetSetting();

            // Trả setting sang View Index.cshtml
            return View(setting);
        }

        // =========================
        // LƯU CÀI ĐẶT
        // =========================

        // Chỉ nhận dữ liệu bằng phương thức POST
        [HttpPost]

        // Chống tấn công CSRF
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(
            Setting model,
            IFormFile? LogoFile)
        {
            // Kiểm tra dữ liệu gửi lên có hợp lệ không
            // ModelState.IsValid sẽ kiểm tra các ràng buộc trong model
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ
                // trả lại trang Index cùng dữ liệu đã nhập
                return View("Index", model);
            }

            // =========================
            // XỬ LÝ UPLOAD LOGO
            // =========================

            // Kiểm tra admin có chọn file logo hay không
            if (LogoFile != null && LogoFile.Length > 0)
            {
                // Tạo đường dẫn thư mục lưu logo:
                // wwwroot/images/Logo
                var folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "Logo"
                );

                // Nếu thư mục Logo chưa tồn tại thì tạo mới
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Tạo tên file ngẫu nhiên bằng Guid
                // giúp tránh trùng tên file khi upload
                var fileName = Guid.NewGuid().ToString()
                               + Path.GetExtension(LogoFile.FileName);

                // Tạo đường dẫn đầy đủ của file trên server
                var filePath = Path.Combine(folder, fileName);

                // Tạo FileStream để ghi file vào server
                using (var stream =
                       new FileStream(filePath, FileMode.Create))
                {
                    // Copy dữ liệu từ file upload vào file trên server
                    await LogoFile.CopyToAsync(stream);
                }

                // Lưu đường dẫn tương đối của logo vào model
                // Đường dẫn này sẽ được lưu xuống database
                // và dùng để hiển thị ảnh trên giao diện
                model.LogoUrl = "/images/Logo/" + fileName;
            }

            // =========================
            // CẬP NHẬT DATABASE
            // =========================

            // Gọi service cập nhật thông tin setting vào database
            _settingService.UpdateSetting(model);

            // Lưu thông báo thành công vào TempData
            // TempData vẫn còn dữ liệu sau khi Redirect
            TempData["Success"] =
                "Cập nhật cài đặt thành công";

            // Sau khi lưu xong, reload lại trang cài đặt
            return RedirectToAction("Index");
        }
    }
}