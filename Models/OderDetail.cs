// File: Models/OrderDetail.cs
// Mô tả:
// Lưu chi tiết từng sản phẩm trong đơn hàng

namespace CNPMFastFood.Models
{
    public class OrderDetail
    {
        // Id chi tiết đơn hàng
        public int Id { get; set; }

        // Id đơn hàng
        public int OrderId { get; set; }

        // Id sản phẩm
        public int ProductId { get; set; }

        // Tên sản phẩm
        public string ProductName { get; set; }

        // Giá sản phẩm
        public decimal Price { get; set; }

        // Số lượng sản phẩm
        public int Quantity { get; set; }

        // Navigation Property
        // dùng để liên kết với bảng Order
        public Order Order { get; set; }
    }
}