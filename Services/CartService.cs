// File: Services/CartService.cs
// Mô tả:
// Xử lý toàn bộ logic giỏ hàng

using CNPMFastFood.Helpers;
using CNPMFastFood.Models;

namespace CNPMFastFood.Services
{
    public class CartService
    {
        // =========================
        // SESSION KEY
        // =========================

        private const string CART_KEY = "cart";

        // =========================
        // HTTP CONTEXT
        // =========================

        private readonly IHttpContextAccessor
            _httpContextAccessor;

        // =========================
        // CONSTRUCTOR
        // =========================

        public CartService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor =
                httpContextAccessor;
        }

        // =========================
        // GET SESSION
        // =========================

        private ISession Session =>
            _httpContextAccessor
                .HttpContext!
                .Session;

        // =========================
        // GET CART
        // Lấy toàn bộ giỏ hàng
        // =========================

        public List<CartItem> GetCart()
        {
            var cart =
                Session.GetObject<List<CartItem>>(
                    CART_KEY);

            // Nếu chưa có cart
            // thì tạo mới

            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            return cart;
        }

        // =========================
        // SAVE CART
        // Lưu cart vào session
        // =========================

        public void SaveCart(
            List<CartItem> cart)
        {
            Session.SetObject(
                CART_KEY,
                cart);
        }

        // =========================
        // ADD TO CART
        // Thêm sản phẩm vào giỏ
        // =========================

        public void AddToCart(
            CartItem item)
        {
            // Lấy cart hiện tại

            var cart = GetCart();

            // Kiểm tra sản phẩm đã tồn tại chưa

            var existingItem =
                cart.FirstOrDefault(
                    x => x.Id == item.Id);

            // Nếu quantity <= 0
            // mặc định = 1

            int quantity =
                item.Quantity <= 0
                ? 1
                : item.Quantity;

            // Nếu sản phẩm đã tồn tại
            // cộng thêm quantity

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }

            // Nếu chưa tồn tại
            // thêm mới vào cart

            else
            {
                item.Quantity = quantity;

                cart.Add(item);
            }

            // Lưu session

            SaveCart(cart);
        }

        // =========================
        // INCREASE
        // Tăng số lượng
        // =========================

        public void Increase(int id)
        {
            var cart = GetCart();

            var item =
                cart.FirstOrDefault(
                    x => x.Id == id);

            if (item != null)
            {
                item.Quantity++;
            }

            SaveCart(cart);
        }

        // =========================
        // DECREASE
        // Giảm số lượng
        // =========================

        public void Decrease(int id)
        {
            var cart = GetCart();

            var item =
                cart.FirstOrDefault(
                    x => x.Id == id);

            if (item != null)
            {
                item.Quantity--;

                // Nếu quantity <= 0
                // xóa khỏi cart

                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            SaveCart(cart);
        }

        // =========================
        // REMOVE
        // Xóa sản phẩm
        // =========================

        public void Remove(int id)
        {
            var cart = GetCart();

            var item =
                cart.FirstOrDefault(
                    x => x.Id == id);

            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCart(cart);
        }

        // =========================
        // TOTAL
        // Tổng tiền
        // =========================

        public decimal GetTotal()
        {
            return GetCart()
                .Sum(x =>
                    x.Price * x.Quantity);
        }

        // =========================
        // COUNT
        // Tổng số lượng sản phẩm
        // =========================

        public int GetCount()
        {
            return GetCart()
                .Sum(x => x.Quantity);
        }

        // =========================
        // CLEAR
        // Xóa toàn bộ cart
        // =========================

        public void Clear()
        {
            SaveCart(
                new List<CartItem>());
        }
    }
}