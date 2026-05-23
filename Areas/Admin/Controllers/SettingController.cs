using CNPMFastFood.Models;
using CNPMFastFood.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class SettingController : Controller
    {
        // Service xử lý lấy/lưu setting
        private readonly SettingService _settingService;

        // Dùng để lấy đường dẫn wwwroot
        private readonly IWebHostEnvironment _environment;

        public SettingController(
            SettingService settingService,
            IWebHostEnvironment environment)
        {
            _settingService = settingService;
            _environment = environment;
        }

        // =========================================
        // HIỂN THỊ TRANG CÀI ĐẶT
        // =========================================

        public IActionResult Index()
        {
            // Lấy setting từ database
            var setting = _settingService.GetSetting();

            return View(setting);
        }

        // =========================================
        // LƯU CÀI ĐẶT
        // =========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(
            Setting model,
            IFormFile? LogoFile)
        {
            // Nếu dữ liệu không hợp lệ
            // thì trả lại view
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // =========================================
            // UPLOAD LOGO
            // =========================================

            // Kiểm tra có chọn file không
            if (LogoFile != null && LogoFile.Length > 0)
            {
                // Tạo đường dẫn:
                // wwwroot/images/Logo
                var folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "Logo"
                );

                // Nếu folder chưa tồn tại
                // thì tự tạo
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Tạo tên file random
                // để tránh trùng tên file
                var fileName = Guid.NewGuid().ToString()
                               + Path.GetExtension(LogoFile.FileName);

                // Đường dẫn đầy đủ để lưu file
                var filePath = Path.Combine(folder, fileName);

                // Copy file vào server
                using (var stream =
                       new FileStream(filePath, FileMode.Create))
                {
                    await LogoFile.CopyToAsync(stream);
                }

                // Lưu đường dẫn vào database
                model.LogoUrl = "/images/Logo/" + fileName;
            }

            // =========================================
            // UPDATE DATABASE
            // =========================================

            _settingService.UpdateSetting(model);

            // Thông báo thành công
            TempData["Success"] =
                "Cập nhật cài đặt thành công";

            // Reload lại trang
            return RedirectToAction("Index");
        }
    }
}