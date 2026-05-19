// ========================================
// FILE: Areas/Admin/Controllers/SettingController.cs
// ========================================

using CNPMFastFood.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // Xác định controller này thuộc Area Admin
    [Area("Admin")]

    // Chỉ tài khoản role admin mới truy cập được
    [Authorize(Roles = "admin")]

    public class SettingController : Controller
    {
        // Service xử lý dữ liệu cài đặt
        private readonly SettingService _settingService;

        // Constructor: inject SettingService
        public SettingController(SettingService settingService)
        {
            _settingService = settingService;
        }

        // =============================
        // HIỂN THỊ TRANG CÀI ĐẶT
        // =============================

        public IActionResult Index()
        {
            // Lấy thông tin cửa hàng
            ViewBag.StoreName =
                _settingService.GetStoreName();

            ViewBag.StoreEmail =
                _settingService.GetStoreEmail();

            ViewBag.StorePhone =
                _settingService.GetStorePhone();

            ViewBag.StoreAddress =
                _settingService.GetStoreAddress();

            ViewBag.OpenTime =
                _settingService.GetOpenTime();

            ViewBag.CloseTime =
                _settingService.GetCloseTime();

            // Lấy cài đặt đơn hàng
            ViewBag.ShippingFee =
                _settingService.GetShippingFee();

            ViewBag.MinimumOrderAmount =
                _settingService.GetMinimumOrderAmount();

            ViewBag.EstimatedDeliveryMinutes =
                _settingService.GetEstimatedDeliveryMinutes();

            // Lấy cài đặt thanh toán
            ViewBag.IsCodEnabled =
                _settingService.IsCodEnabled();

            ViewBag.IsBankTransferEnabled =
                _settingService.IsBankTransferEnabled();

            return View();
        }

        // =============================
        // XỬ LÝ LƯU CÀI ĐẶT
        // =============================

        [HttpPost]
        public IActionResult Save()
        {
            // Hiện tại mới demo giao diện
            // Sau này sẽ nhận dữ liệu từ form và lưu vào database

            TempData["Success"] =
                "Lưu cài đặt thành công";

            return RedirectToAction("Index");
        }
    }
}