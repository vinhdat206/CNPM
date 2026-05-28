// Sử dụng namespace chứa AppDbContext để làm việc với database
using CNPMFastFood.Data;

// Sử dụng namespace chứa các model như Cart, CartItem
using CNPMFastFood.Models;

// Khai báo namespace chứa các service của project
namespace CNPMFastFood.Services
{
    // CartService là lớp xử lý toàn bộ nghiệp vụ liên quan đến giỏ hàng
    public class CartService
    {
        // Đối tượng DbContext dùng để truy vấn và cập nhật database
        private readonly AppDbContext _context;

        // Service dùng để lấy các cài đặt hệ thống, ví dụ phí vận chuyển
        private readonly SettingService _settingService;

        // Constructor nhận AppDbContext và SettingService thông qua Dependency Injection
        public CartService(
            AppDbContext context,
            SettingService settingService)
        {
            // Gán DbContext được truyền vào cho biến _context
            _context = context;

            // Gán SettingService được truyền vào cho biến _settingService
            _settingService = settingService;
        }

        // Lấy giỏ hàng của một người dùng theo userId
        public Cart GetCart(int userId)
        {
            // Lấy danh sách sản phẩm trong bảng CartItems
            // Chỉ lấy những sản phẩm thuộc về người dùng đang đăng nhập
            var items = _context.CartItems
                .Where(x => x.UserId == userId)
                .ToList();

            // Tạo đối tượng Cart mới để trả về cho Controller/View
            return new Cart
            {
                // Gán UserId cho giỏ hàng, chuyển từ int sang string
                UserId = userId.ToString(),

                // Gán danh sách sản phẩm vừa lấy được
                Items = items,

                // Gán phí vận chuyển lấy từ cài đặt hệ thống
                ShippingFee = GetShippingFee()
            };
        }

        // Thêm sản phẩm vào giỏ hàng
        public void AddToCart(
            CartItem item,
            int userId)
        {
            // Kiểm tra xem sản phẩm này đã tồn tại trong giỏ hàng của user chưa
            var existingItem = _context.CartItems
                .FirstOrDefault(x =>
                    x.UserId == userId &&
                    x.ProductId == item.ProductId);

            // Nếu sản phẩm đã có trong giỏ hàng
            if (existingItem != null)
            {
                // Cộng thêm số lượng mới vào số lượng cũ
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                // Nếu sản phẩm chưa có trong giỏ hàng
                // Gán UserId cho sản phẩm để biết sản phẩm này thuộc giỏ hàng của ai
                item.UserId = userId;

                // Thêm sản phẩm mới vào bảng CartItems
                _context.CartItems.Add(item);
            }

            // Lưu tất cả thay đổi xuống database
            _context.SaveChanges();
        }

        // Tăng số lượng sản phẩm trong giỏ hàng
        public void Increase(
            int productId,
            int userId)
        {
            // Tìm sản phẩm trong giỏ hàng theo userId và productId
            var item = _context.CartItems
                .FirstOrDefault(x =>
                    x.UserId == userId &&
                    x.ProductId == productId);

            // Nếu không tìm thấy sản phẩm thì dừng hàm
            if (item == null)
                return;

            // Tăng số lượng sản phẩm lên 1
            item.Quantity++;

            // Lưu thay đổi xuống database
            _context.SaveChanges();
        }

        // Giảm số lượng sản phẩm trong giỏ hàng
        public void Decrease(
            int productId,
            int userId)
        {
            // Tìm sản phẩm trong giỏ hàng theo userId và productId
            var item = _context.CartItems
                .FirstOrDefault(x =>
                    x.UserId == userId &&
                    x.ProductId == productId);

            // Nếu không tìm thấy sản phẩm thì dừng hàm
            if (item == null)
                return;

            // Giảm số lượng sản phẩm đi 1
            item.Quantity--;

            // Nếu số lượng sau khi giảm nhỏ hơn hoặc bằng 0
            if (item.Quantity <= 0)
            {
                // Xóa sản phẩm khỏi giỏ hàng
                _context.CartItems.Remove(item);
            }

            // Lưu thay đổi xuống database
            _context.SaveChanges();
        }

        // Xóa một sản phẩm khỏi giỏ hàng
        public void Remove(
            int productId,
            int userId)
        {
            // Tìm sản phẩm cần xóa trong giỏ hàng của user
            var item = _context.CartItems
                .FirstOrDefault(x =>
                    x.UserId == userId &&
                    x.ProductId == productId);

            // Nếu không tìm thấy sản phẩm thì dừng hàm
            if (item == null)
                return;

            // Xóa sản phẩm khỏi bảng CartItems
            _context.CartItems.Remove(item);

            // Lưu thay đổi xuống database
            _context.SaveChanges();
        }

        // Xóa toàn bộ giỏ hàng của một user
        public void Clear(int userId)
        {
            // Lấy tất cả sản phẩm trong giỏ hàng của user
            var items = _context.CartItems
                .Where(x => x.UserId == userId)
                .ToList();

            // Xóa toàn bộ danh sách sản phẩm vừa lấy
            _context.CartItems.RemoveRange(items);

            // Lưu thay đổi xuống database
            _context.SaveChanges();
        }

        // Tính tổng tiền hàng trong giỏ
        public decimal GetTotal(int userId)
        {
            // Lấy các sản phẩm thuộc userId
            // Sau đó tính tổng: giá sản phẩm * số lượng
            return _context.CartItems
                .Where(x => x.UserId == userId)
                .Sum(x => x.Price * x.Quantity);
        }

        // Lấy phí vận chuyển
        public decimal GetShippingFee()
        {
            // Lấy thông tin cài đặt hệ thống
            var setting = _settingService.GetSetting();

            // Trả về phí vận chuyển đang được cấu hình
            return setting.ShippingFee;
        }

        // Tính tổng tiền cuối cùng
        public decimal GetGrandTotal(int userId)
        {
            // Tổng cuối cùng = tổng tiền hàng + phí vận chuyển
            return GetTotal(userId)
                   + GetShippingFee();
        }

        // Đếm tổng số lượng sản phẩm trong giỏ hàng
        public int GetCount(int userId)
        {
            // Lấy các sản phẩm thuộc userId
            // Sau đó cộng tổng Quantity của tất cả sản phẩm
            return _context.CartItems
                .Where(x => x.UserId == userId)
                .Sum(x => x.Quantity);
        }
    }
}