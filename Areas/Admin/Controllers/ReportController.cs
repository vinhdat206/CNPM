// ========================================
// FILE: Areas/Admin/Controllers/ReportController.cs
// ========================================

using CNPMFastFood.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    [Area("Admin")]

    // Chỉ admin mới được vào
    [Authorize(Roles = "admin")]

    public class ReportController : Controller
    {
        // Service
        private readonly ReportService _reportService;

        // Constructor
        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        // =============================
        // REPORT PAGE
        // =============================

        public IActionResult Index()
        {
            // Top sản phẩm bán chạy
            ViewBag.TopProductNames =
                _reportService.GetTopProductNames();

            ViewBag.TopProductSales =
                _reportService.GetTopProductSales();

            // Trạng thái đơn hàng
            ViewBag.OrderStatusLabels =
                _reportService.GetOrderStatusLabels();

            ViewBag.OrderStatusValues =
                _reportService.GetOrderStatusValues();

            // Doanh thu theo sản phẩm
            ViewBag.ProductRevenueNames =
                _reportService.GetProductRevenueNames();

            ViewBag.ProductRevenueValues =
                _reportService.GetProductRevenueValues();

            // Top khách hàng
            ViewBag.TopCustomers =
                _reportService.GetTopCustomers();

            return View();
        }
    }
}