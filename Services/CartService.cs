using CNPMFastFood.Data;
using CNPMFastFood.Models;

namespace CNPMFastFood.Services
{
    public class CartService
    {
        private readonly AppDbContext _context;
        private readonly SettingService _settingService;

        public CartService(
            AppDbContext context,
            SettingService settingService)
        {
            _context = context;
            _settingService = settingService;
        }

        // GET CART

        public Cart GetCart(int userId)
        {
            var items = _context.CartItems
                .Where(x => x.UserId == userId)
                .ToList();

            return new Cart
            {
                UserId = userId.ToString(),
                Items = items,
                ShippingFee = GetShippingFee()
            };
        }

        // ADD TO CART

        public void AddToCart(
            CartItem item,
            int userId)
        {
            var existingItem = _context.CartItems
                .FirstOrDefault(x =>
                    x.UserId == userId &&
                    x.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                item.UserId = userId;

                _context.CartItems.Add(item);
            }

            _context.SaveChanges();
        }

        // INCREASE

        public void Increase(
            int productId,
            int userId)
        {
            var item = _context.CartItems
                .FirstOrDefault(x =>
                    x.UserId == userId &&
                    x.ProductId == productId);

            if (item == null)
                return;

            item.Quantity++;

            _context.SaveChanges();
        }

        // DECREASE

        public void Decrease(
            int productId,
            int userId)
        {
            var item = _context.CartItems
                .FirstOrDefault(x =>
                    x.UserId == userId &&
                    x.ProductId == productId);

            if (item == null)
                return;

            item.Quantity--;

            if (item.Quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }

            _context.SaveChanges();
        }

        // REMOVE

        public void Remove(
            int productId,
            int userId)
        {
            var item = _context.CartItems
                .FirstOrDefault(x =>
                    x.UserId == userId &&
                    x.ProductId == productId);

            if (item == null)
                return;

            _context.CartItems.Remove(item);

            _context.SaveChanges();
        }

        // CLEAR

        public void Clear(int userId)
        {
            var items = _context.CartItems
                .Where(x => x.UserId == userId)
                .ToList();

            _context.CartItems.RemoveRange(items);

            _context.SaveChanges();
        }

        // TOTAL

        public decimal GetTotal(int userId)
        {
            return _context.CartItems
                .Where(x => x.UserId == userId)
                .Sum(x => x.Price * x.Quantity);
        }

        // SHIPPING

        public decimal GetShippingFee()
        {
            var setting = _settingService.GetSetting();

            return setting.ShippingFee;
        }

        // GRAND TOTAL

        public decimal GetGrandTotal(int userId)
        {
            return GetTotal(userId)
                   + GetShippingFee();
        }

        // COUNT

        public int GetCount(int userId)
        {
            return _context.CartItems
                .Where(x => x.UserId == userId)
                .Sum(x => x.Quantity);
        }
    }
}