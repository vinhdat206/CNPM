// File: Controllers/OrderController.cs
// Mô tả:
// Controller xử lý:
// - Trang checkout
// - Đặt hàng
// - Trang đặt hàng thành công
// - Lịch sử đơn hàng

using Microsoft.AspNetCore.Authorization; // Dùng để yêu cầu người dùng phải đăng nhập
using Microsoft.AspNetCore.Mvc; // Dùng cho Controller, IActionResult, View,...

using CNPMFastFood.Models; // Chứa model Order
using CNPMFastFood.Services; // Chứa CartService và OrderService

namespace CNPMFastFood.Controllers
{
    // Yêu cầu người dùng phải đăng nhập mới được truy cập controller này
    [Authorize]
    public class OrderController : Controller
    {
        // Service xử lý giỏ hàng
        private readonly CartService _cartService;

        // Service xử lý đơn hàng
        private readonly OrderService _orderService;

        // Constructor Dependency Injection
        public OrderController(
            CartService cartService,
            OrderService orderService)
        {
            // Gán CartService
            _cartService = cartService;

            // Gán OrderService
            _orderService = orderService;
        }

        // =========================
        // LẤY USER ID
        // =========================

        private int GetUserId()
        {
            // Lấy UserId từ Claim đã lưu khi đăng nhập
            return int.Parse(
                User.FindFirst("UserId")!.Value
            );
        }

        // =========================
        // CHECKOUT PAGE
        // Hiển thị trang thanh toán
        // =========================

        public IActionResult Checkout()
        {
            // Lấy id người dùng hiện tại
            int userId = GetUserId();

            // Lấy giỏ hàng của người dùng
            var cart =
                _cartService.GetCart(userId);

            // Tính phí giao hàng
            cart.ShippingFee =
                _cartService.GetShippingFee();

            // Truyền giỏ hàng sang View Checkout.cshtml
            return View(cart);
        }

        // =========================
        // PLACE ORDER
        // Xử lý đặt hàng
        // =========================

        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            // Lấy id người dùng hiện tại
            int userId = GetUserId();

            // Lấy giỏ hàng của người dùng
            var cart =
                _cartService.GetCart(userId);

            // Tính phí giao hàng
            cart.ShippingFee =
                _cartService.GetShippingFee();

            // Nếu giỏ hàng rỗng thì quay về trang Cart
            if (cart.Items.Count == 0)
            {
                return RedirectToAction(
                    "Index",
                    "Cart");
            }

            // Tạo đơn hàng từ thông tin order và các sản phẩm trong giỏ
            _orderService.CreateOrder(
                order,             // Thông tin người nhận / đơn hàng
                cart.Items,        // Danh sách sản phẩm trong giỏ
                userId,            // Id người dùng
                cart.ShippingFee); // Phí giao hàng

            // Xóa giỏ hàng sau khi đặt hàng thành công
            _cartService.Clear(userId);

            // Chuyển sang trang đặt hàng thành công
            return RedirectToAction(
                "Success");
        }

        // =========================
        // SUCCESS PAGE
        // Trang thông báo đặt hàng thành công
        // =========================

        public IActionResult Success()
        {
            // Trả về View Success.cshtml
            return View();
        }

        // =========================
        // ORDER HISTORY
        // Hiển thị lịch sử đơn hàng
        // =========================

        public async Task<IActionResult> History()
        {
            // Lấy id người dùng hiện tại
            int userId = GetUserId();

            // Lấy danh sách đơn hàng của người dùng
            var orders =
                await _orderService
                    .GetOrderHistory(userId);

            // Truyền danh sách đơn hàng sang View History.cshtml
            return View(orders);
        }
    }
}