// ========================================
// FILE: Areas/Admin/Controllers/ReportController.cs
// ========================================

// Import namespace chứa ReportService
using CNPMFastFood.Services;

// Import thư viện phân quyền
using Microsoft.AspNetCore.Authorization;

// Import thư viện ASP.NET Core MVC
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // =====================================================
    // Controller thuộc khu vực Admin
    // URL mặc định:
    // /Admin/Report/Index
    // =====================================================

    [Area("Admin")]

    // =====================================================
    // Chỉ tài khoản có role "admin"
    // mới được truy cập controller này
    //
    // Nếu:
    // - Chưa đăng nhập -> chuyển tới Login
    // - Không phải admin -> báo Access Denied
    // =====================================================

    [Authorize(Roles = "admin")]

    public class ReportController : Controller
    {
        // =====================================================
        // Service xử lý nghiệp vụ thống kê/báo cáo
        // =====================================================

        private readonly ReportService _reportService;

        // =====================================================
        // Constructor
        // Dependency Injection sẽ tự động inject ReportService
        // =====================================================

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        // =====================================================
        // TRANG BÁO CÁO THỐNG KÊ
        // =====================================================

        public IActionResult Index()
        {
            // =================================================
            // TOP SẢN PHẨM BÁN CHẠY
            // =================================================

            // Lấy danh sách tên sản phẩm bán chạy
            // Ví dụ:
            // ["Burger", "Pizza", "Trà sữa"]
            ViewBag.TopProductNames =
                _reportService.GetTopProductNames();

            // Lấy số lượng bán tương ứng
            // Ví dụ:
            // [120, 90, 75]
            ViewBag.TopProductSales =
                _reportService.GetTopProductSales();

            // =================================================
            // THỐNG KÊ TRẠNG THÁI ĐƠN HÀNG
            // =================================================

            // Lấy tên các trạng thái đơn hàng
            // Ví dụ:
            // ["Pending", "Completed", "Cancelled"]
            ViewBag.OrderStatusLabels =
                _reportService.GetOrderStatusLabels();

            // Lấy số lượng đơn hàng theo trạng thái
            // Ví dụ:
            // [15, 120, 8]
            ViewBag.OrderStatusValues =
                _reportService.GetOrderStatusValues();

            // =================================================
            // DOANH THU THEO SẢN PHẨM
            // =================================================

            // Lấy danh sách tên sản phẩm
            ViewBag.ProductRevenueNames =
                _reportService.GetProductRevenueNames();

            // Lấy doanh thu tương ứng của từng sản phẩm
            // Ví dụ:
            // [2500000, 1800000, 3200000]
            ViewBag.ProductRevenueValues =
                _reportService.GetProductRevenueValues();

            // =================================================
            // TOP KHÁCH HÀNG
            // =================================================

            // Lấy danh sách khách hàng mua nhiều nhất
            // Có thể gồm:
            // - Tên khách hàng
            // - Tổng tiền đã mua
            // - Số đơn hàng
            ViewBag.TopCustomers =
                _reportService.GetTopCustomers();

            // =================================================
            // Trả về View Index.cshtml
            // =================================================

            return View();
        }
    }
}