// File: Controllers/OrderController.cs
// Mô tả:
// Điều hướng trang checkout + đặt hàng

using Microsoft.AspNetCore.Mvc;

using CNPMFastFood.Models;
using CNPMFastFood.Services;

namespace CNPMFastFood.Controllers
{
    public class OrderController : Controller
    {
        // Service giỏ hàng
        private readonly CartService _cartService;

        // Service đặt hàng
        private readonly OrderService _orderService;

        // Constructor
        public OrderController(
            CartService cartService,
            OrderService orderService)
        {
            _cartService = cartService;

            _orderService = orderService;
        }

        // =========================
        // CHECKOUT PAGE
        // =========================

        public IActionResult Checkout()
        {
            // lấy cart hiện tại
            var cart = _cartService.GetCart();

            // truyền cart sang view
            return View(cart);
        }

        // =========================
        // PLACE ORDER
        // =========================

        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            // lấy cart
            var cart = _cartService.GetCart();

            // nếu cart rỗng
            if (cart.Count == 0)
            {
                return RedirectToAction(
                    "Index",
                    "Cart");
            }

            // gọi service tạo order
            var userId = int.Parse( User.FindFirst("UserId")!.Value); 
            _orderService.CreateOrder( order, cart, userId);

            // xóa cart sau khi đặt hàng
            _cartService.Clear();

            // chuyển sang trang success
            return RedirectToAction("Success");
        }

        // =========================
        // SUCCESS PAGE
        // =========================

        public IActionResult Success()
        {
            return View();
        }
        
        
        // ===============================
        // ORDER HISTORY
        // ===============================

        public async Task<IActionResult> History()
        {
            // Kiểm tra login
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            // Lấy UserId từ Claims
            var userIdClaim =
                User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            // Convert sang int
            int userId = int.Parse(userIdClaim);

            // Lấy danh sách order
            var orders =
                await _orderService
                    .GetOrderHistory(userId);

            // Trả view
            return View(orders);
        }


    }
}