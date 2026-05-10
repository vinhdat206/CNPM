// File: Services/CartService.cs
// Mô tả: Xử lý logic giỏ hàng

using CNPMFastFood.Helpers;
using CNPMFastFood.Models;

namespace CNPMFastFood.Services
{
    public class CartService
    {
        // dùng để lấy Session
        private readonly IHttpContextAccessor
            _httpContextAccessor;

        // constructor
        public CartService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor =
                httpContextAccessor;
        }

        // ================= GET SESSION =================

        private ISession Session =>
            _httpContextAccessor
                .HttpContext
                .Session;

        // ================= GET CART =================

        public List<CartItem> GetCart()
        {
            // lấy cart từ session
            var cart =
                Session.GetObject<List<CartItem>>(
                    "cart");

            // nếu chưa có cart
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            return cart;
        }

        // ================= SAVE CART =================

        public void SaveCart(
            List<CartItem> cart)
        {
            Session.SetObject(
                "cart",
                cart);
        }

        // ================= ADD TO CART =================

        public void AddToCart(
            CartItem item)
        {
            var cart = GetCart();

            // tìm sản phẩm đã tồn tại chưa
            var existingItem =
                cart.FirstOrDefault(
                    x => x.Id == item.Id);

            // nếu có rồi
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                // nếu chưa có
                item.Quantity = 1;

                cart.Add(item);
            }

            SaveCart(cart);
        }

        // ================= INCREASE =================

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

        // ================= DECREASE =================

        public void Decrease(int id)
        {
            var cart = GetCart();

            var item =
                cart.FirstOrDefault(
                    x => x.Id == id);

            if (item != null)
            {
                item.Quantity--;

                // nếu quantity <= 0
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            SaveCart(cart);
        }

        // ================= REMOVE =================

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

        // ================= TOTAL =================

        public decimal GetTotal()
        {
            return GetCart()
                .Sum(x =>
                    x.Price * x.Quantity);
        }

        // ================= COUNT =================

        public int GetCount()
        {
            return GetCart()
                .Sum(x => x.Quantity);
        }
        // ================= CLEAR CART =================

// xóa toàn bộ giỏ hàng
        public void Clear()
        {
            // tạo cart rỗng
            var emptyCart =
                new List<CartItem>();

            // lưu cart rỗng vào session
            SaveCart(emptyCart);
        }
    }
}