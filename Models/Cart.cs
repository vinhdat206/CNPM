// File: Models/Cart.cs
// Mô tả: Giỏ hàng

using System.Collections.Generic;

namespace CNPMFastFood.Models
{
    public class Cart
    {
        // BẮT BUỘC phải có khóa chính
        public int Id { get; set; }

        // Danh sách sản phẩm
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}