using CNPMFastFood.Services;
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: /Admin/Order
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Đơn hàng";
            ViewData["PageTitle"] = "Quản lý đơn hàng";

            var orders = await _orderService.GetAllOrdersAsync();

            return View(orders);
        }

        // GET: /Admin/Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewData["Title"] = "Chi tiết đơn hàng";
            ViewData["PageTitle"] = "Chi tiết đơn hàng";

            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        // POST: /Admin/Order/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                TempData["Error"] = "Trạng thái không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            await _orderService.UpdateStatusAsync(id, status);

            TempData["Success"] = "Cập nhật trạng thái đơn hàng thành công!";

            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Order/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await _orderService.CancelOrderAsync(id);

            TempData["Success"] = "Đã hủy đơn hàng!";

            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Order/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);

            TempData["Success"] = "Đã xóa đơn hàng!";

            return RedirectToAction(nameof(Index));
        }
    }
}