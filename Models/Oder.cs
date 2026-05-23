// File: Models/Order.cs

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

        // Tổng giá sản phẩm
        public decimal SubTotal { get; set; }

        // Phí ship
        public decimal ShippingFee { get; set; }

        // Tổng tiền cuối cùng
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
        public string? CancelReason { get; set; }
        public string? PaymentMethod { get; set; }

        // Danh sách chi tiết đơn hàng
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}