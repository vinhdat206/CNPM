// File: Models/Order.cs
// Mô tả:
// Lưu thông tin đơn hàng của khách hàng

namespace CNPMFastFood.Models
{
    public class Order
    {
        // Id đơn hàng
        public int Id { get; set; }

        // Tên khách hàng
        public string CustomerName { get; set; }

        // Số điện thoại khách
        public string Phone { get; set; }

        // Địa chỉ giao hàng
        public string Address { get; set; }

        // Tổng tiền đơn hàng
        public decimal TotalAmount { get; set; }

        // Ngày đặt hàng
        public DateTime OrderDate { get; set; }

        // Trạng thái đơn hàng
        // Ví dụ:
        // Pending
        // Shipping
        // Completed
        public string Status { get; set; }

        // Danh sách chi tiết đơn hàng
        // 1 Order có nhiều OrderDetail
        public List<OrderDetail> OrderDetails { get; set; }
    }
}