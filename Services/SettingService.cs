// =============================
// FILE: Services/SettingService.cs
// =============================

using CNPMFastFood.Data;

namespace CNPMFastFood.Services
{
    public class SettingService
    {
        // AppDbContext dùng để làm việc với database
        private readonly AppDbContext _context;

        // Constructor: inject AppDbContext vào service
        public SettingService(AppDbContext context)
        {
            _context = context;
        }

        // =============================
        // THÔNG TIN CỬA HÀNG
        // =============================

        // Lấy tên cửa hàng
        public string GetStoreName()
        {
            return "CNPM Fast Food";
        }

        // Lấy email cửa hàng
        public string GetStoreEmail()
        {
            return "fastfood@gmail.com";
        }

        // Lấy số điện thoại cửa hàng
        public string GetStorePhone()
        {
            return "0123 456 789";
        }

        // Lấy địa chỉ cửa hàng
        public string GetStoreAddress()
        {
            return "Ha Noi ";
        }

        // Lấy giờ mở cửa
        public string GetOpenTime()
        {
            return "08:00";
        }

        // Lấy giờ đóng cửa
        public string GetCloseTime()
        {
            return "22:00";
        }

        // =============================
        // CÀI ĐẶT ĐƠN HÀNG
        // =============================

        // Lấy phí giao hàng
        public decimal GetShippingFee()
        {
            return 20000;
        }

        // Lấy giá trị đơn hàng tối thiểu
        public decimal GetMinimumOrderAmount()
        {
            return 50000;
        }

        // Lấy thời gian giao hàng dự kiến
        public int GetEstimatedDeliveryMinutes()
        {
            return 30;
        }

        // =============================
        // CÀI ĐẶT THANH TOÁN
        // =============================

        // Kiểm tra có bật thanh toán COD không
        public bool IsCodEnabled()
        {
            return true;
        }

        // Kiểm tra có bật chuyển khoản ngân hàng không
        public bool IsBankTransferEnabled()
        {
            return false;
        }
    }
}