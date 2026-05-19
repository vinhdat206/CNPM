// File: Models/Order.cs
// Mô tả:
// Lưu thông tin đơn hàng của khách hàng

namespace CNPMFastFood.Models
{
    public class Order
    {
        // Id đơn hàng
        public int Id { get; set; }

        // Id người dùng
        public int UserId { get; set; }

        // Tên khách hàng
        public string CustomerName { get; set; } = string.Empty;

        // Số điện thoại khách
        public string Phone { get; set; } = string.Empty;

        // Địa chỉ giao hàng
        public string Address { get; set; } = string.Empty;

        // Tổng tiền đơn hàng
        public decimal TotalAmount { get; set; }

        // Ngày đặt hàng
        public DateTime OrderDate { get; set; }

        // Trạng thái đơn hàng
        // Pending
        // Processing
        // Shipping
        // Completed
        // Cancelled
        public string Status { get; set; } = "Pending";

        // Danh sách chi tiết đơn hàng
        // 1 Order có nhiều OrderDetail
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}