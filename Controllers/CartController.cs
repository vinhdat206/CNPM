// File: Controllers/CartController.cs
// Mô tả:
// Điều hướng giỏ hàng

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CNPMFastFood.Models;
using CNPMFastFood.Services;

namespace CNPMFastFood.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        // =========================
        // CART PAGE
        // =========================

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();

            return View(cart);
        }

        // =========================
        // ADD TO CART
        // =========================

        public IActionResult Add(
            int id,
            string name,
            decimal price,
            string imageUrl,
            int quantity = 1)
        {
            var item = new CartItem
            {
                Id = id,
                Name = name,
                Price = price,
                ImageUrl = imageUrl,
                Quantity = quantity
            };

            _cartService.AddToCart(item);

            return RedirectToAction("Index", "Cart");
        }

        // =========================
        // INCREASE QUANTITY
        // AJAX
        // =========================

        [HttpPost]
        public IActionResult Increase(int id)
        {
            _cartService.Increase(id);

            return Json(new
            {
                success = true,
                cart = _cartService.GetCart(),
                total = _cartService.GetTotal(),
                count = _cartService.GetCount()
            });
        }

        // =========================
        // DECREASE QUANTITY
        // AJAX
        // =========================

        [HttpPost]
        public IActionResult Decrease(int id)
        {
            _cartService.Decrease(id);

            return Json(new
            {
                success = true,
                cart = _cartService.GetCart(),
                total = _cartService.GetTotal(),
                count = _cartService.GetCount()
            });
        }

        // =========================
        // REMOVE ITEM
        // AJAX
        // =========================

        [HttpPost]
        public IActionResult Remove(int id)
        {
            _cartService.Remove(id);

            return Json(new
            {
                success = true,
                cart = _cartService.GetCart(),
                total = _cartService.GetTotal(),
                count = _cartService.GetCount()
            });
        }

        // =========================
        // CLEAR CART
        // =========================

        public IActionResult Clear()
        {
            _cartService.Clear();

            return RedirectToAction("Index", "Cart");
        }
    }
}