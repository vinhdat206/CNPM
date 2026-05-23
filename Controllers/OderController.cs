// File: Controllers/OrderController.cs
// Mô tả:
// Điều hướng trang checkout + đặt hàng

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CNPMFastFood.Models;
using CNPMFastFood.Services;

namespace CNPMFastFood.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly CartService _cartService;

        private readonly OrderService _orderService;

        public OrderController(
            CartService cartService,
            OrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        private int GetUserId()
        {
            return int.Parse(
                User.FindFirst("UserId")!.Value
            );
        }

        public IActionResult Checkout()
        {
            int userId = GetUserId();

            var cart =
                _cartService.GetCart(userId);

            cart.ShippingFee =
                _cartService.GetShippingFee();

            return View(cart);
        }

        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            int userId = GetUserId();

            var cart =
                _cartService.GetCart(userId);

            cart.ShippingFee =
                _cartService.GetShippingFee();

            if (cart.Items.Count == 0)
            {
                return RedirectToAction(
                    "Index",
                    "Cart");
            }

            _orderService.CreateOrder(
                order,
                cart.Items,
                userId,
                cart.ShippingFee);

            _cartService.Clear(userId);

            return RedirectToAction(
                "Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> History()
        {
            int userId = GetUserId();

            var orders =
                await _orderService
                    .GetOrderHistory(userId);

            return View(orders);
        }
    }
}