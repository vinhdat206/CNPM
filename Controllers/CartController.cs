// File: Controllers/CartController.cs

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

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new Exception("Không tìm thấy UserId trong tài khoản đăng nhập.");
            }

            return int.Parse(userIdClaim);
        }

        public IActionResult Index()
        {
            int userId = GetUserId();

            var cart = _cartService.GetCart(userId);

            return View(cart);
        }

        [HttpPost]
        public IActionResult Add(
            int id,
            string name,
            decimal price,
            string imageUrl,
            int quantity = 1)
        {
            int userId = GetUserId();

            var item = new CartItem
            {
                ProductId = id,
                Name = name,
                Price = price,
                ImageUrl = imageUrl,
                Quantity = quantity
            };

            _cartService.AddToCart(item, userId);
            HttpContext.Session.SetString(
                "CartCount",
                _cartService.GetCount(userId).ToString()
            );

            return JsonResult(userId);
        }

        [HttpPost]
        public IActionResult Increase(int id)
        {
            int userId = GetUserId();

            _cartService.Increase(id, userId);

            return JsonResult(userId);
        }

        [HttpPost]
        public IActionResult Decrease(int id)
        {
            int userId = GetUserId();

            _cartService.Decrease(id, userId);

            return JsonResult(userId);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            int userId = GetUserId();

            _cartService.Remove(id, userId);

            return JsonResult(userId);
        }

        public IActionResult Clear()
        {
            int userId = GetUserId();

            _cartService.Clear(userId);

            return RedirectToAction("Index", "Cart");
        }

        private JsonResult JsonResult(int userId)
        {
            var cart = _cartService.GetCart(userId);

            return Json(new
            {
                success = true,
                cart,
                total = _cartService.GetTotal(userId),
                shippingFee = _cartService.GetShippingFee(),
                grandTotal = _cartService.GetGrandTotal(userId),
                count = _cartService.GetCount(userId)
            });
        }
    }
}