using CNPMFastFood.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            ViewBag.TotalRevenue = _dashboardService.GetTotalRevenue();
            ViewBag.TotalOrders = _dashboardService.GetTotalOrders();
            ViewBag.TotalCustomers = _dashboardService.GetTotalCustomers();
            ViewBag.MonthlyRevenue = _dashboardService.GetMonthlyRevenue();

            return View();
        }
    }
}