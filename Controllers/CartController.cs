using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CNPMFastFood.Models;
using CNPMFastFood.Services;
using CNPMFastFood.Data;

namespace CNPMFastFood.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly AppDbContext _context;

        public CartController(
            CartService cartService,
            AppDbContext context)
        {
            _cartService = cartService;
            _context = context;
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

            var product = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Không tìm thấy sản phẩm"
                });
            }

            if (product.Stock <= 0)
            {
                return Json(new
                {
                    success = false,
                    message = "Sản phẩm đã hết hàng"
                });
            }

            if (quantity > product.Stock)
            {
                return Json(new
                {
                    success = false,
                    message = $"Chỉ còn {product.Stock} sản phẩm"
                });
            }

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

            var product = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Không tìm thấy sản phẩm"
                });
            }

            var cart = _cartService.GetCart(userId);

            var cartItem = cart.Items
                .FirstOrDefault(x => x.ProductId == id);

            if (cartItem != null && cartItem.Quantity >= product.Stock)
            {
                return Json(new
                {
                    success = false,
                    message = $"Chỉ còn {product.Stock} sản phẩm"
                });
            }

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

        [HttpGet]
        public IActionResult Count()
        {
            int userId = GetUserId();

            return Json(new
            {
                success = true,
                count = _cartService.GetCount(userId)
            });
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