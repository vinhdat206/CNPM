// File: Models/CartItem.cs
// Mô tả: Item trong giỏ hàng

namespace CNPMFastFood.Models
{
    public class CartItem
    {
        // Id sản phẩm
        public int Id { get; set; }

        // Tên sản phẩm
        public string Name { get; set; }

        // Giá sản phẩm
        public decimal Price { get; set; }

        // Số lượng
        public int Quantity { get; set; }

        // Ảnh sản phẩm
        public string ImageUrl { get; set; }
    }
}