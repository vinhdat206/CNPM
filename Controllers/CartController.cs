// File: Controllers/CartController.cs
// Mô tả:
// Điều hướng giỏ hàng

using Microsoft.AspNetCore.Mvc;

using CNPMFastFood.Models;
using CNPMFastFood.Services;

namespace CNPMFastFood.Controllers
{
    public class CartController : Controller
    {
        // Service cart
        private readonly CartService
            _cartService;

        // Constructor
        public CartController(
            CartService cartService)
        {
            _cartService = cartService;
        }

        // =========================
        // CART PAGE
        // =========================

        public IActionResult Index()
        {
            // lấy cart từ session
            var cart =
                _cartService.GetCart();

            // truyền sang view
            return View(cart);
        }

        // =========================
        // ADD TO CART
        // =========================

        public IActionResult Add(
            int id,
            string name,
            decimal price,
            string imageUrl)
        {
            // tạo item mới
            var item = new CartItem
            {
                Id = id,

                Name = name,

                Price = price,

                ImageUrl = imageUrl
            };

            // thêm vào cart
            _cartService.AddToCart(item);

            // chuyển sang cart
            return RedirectToAction(
                "Index",
                "Cart");
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
            // xóa session cart
            HttpContext.Session.Remove("CART");

            // reload cart
            return RedirectToAction("Index");
        }
    }
}