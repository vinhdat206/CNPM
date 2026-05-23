using System.Collections.Generic;
using System.Linq;

namespace CNPMFastFood.Models
{
    public class Cart
    {
        // khóa chính
        public int Id { get; set; }

        // user đăng nhập
        public string? UserId { get; set; }

        // danh sách sản phẩm
        public List<CartItem> Items { get; set; }
            = new List<CartItem>();

        // phí ship
        public decimal ShippingFee { get; set; } = 30000;

        // tạm tính
        public decimal SubTotal =>
            Items.Sum(x => x.Price * x.Quantity);

        // tổng tiền
        public decimal GrandTotal =>
            SubTotal + ShippingFee;
    }
}