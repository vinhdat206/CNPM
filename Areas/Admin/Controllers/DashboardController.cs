// Import service xử lý dữ liệu thống kê Dashboard
using CNPMFastFood.Services;

// Import thư viện phân quyền trong ASP.NET Core
using Microsoft.AspNetCore.Authorization;

// Import thư viện MVC để sử dụng Controller, IActionResult, ViewBag...
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // Controller này thuộc khu vực Admin
    // URL thường có dạng: /Admin/Dashboard/Index
    [Area("Admin")]

    // Chỉ cho phép tài khoản có role là "admin" truy cập Controller này
    // Nếu user chưa đăng nhập hoặc không phải admin thì sẽ bị chặn
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        // Khai báo service dùng để lấy dữ liệu thống kê
        private readonly DashboardService _dashboardService;

        // Constructor
        // ASP.NET Core sẽ tự động inject DashboardService vào Controller
        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Action hiển thị trang Dashboard
        public IActionResult Index()
        {
            // Lấy tổng doanh thu từ service
            // Dữ liệu này sẽ được truyền sang View qua ViewBag
            ViewBag.TotalRevenue = _dashboardService.GetTotalRevenue();

            // Lấy tổng số đơn hàng
            ViewBag.TotalOrders = _dashboardService.GetTotalOrders();

            // Lấy tổng số khách hàng đã đăng ký
            ViewBag.TotalCustomers = _dashboardService.GetTotalCustomers();

            // Lấy doanh thu theo từng tháng
            // Thường dùng để hiển thị biểu đồ doanh thu
            ViewBag.MonthlyRevenue = _dashboardService.GetMonthlyRevenue();

            // Trả về View Index.cshtml trong thư mục Dashboard
            return View();
        }
    }
}