using CNPMFastFood.Data;
using CNPMFastFood.Models;

namespace CNPMFastFood.Services
{
    public class SettingService
    {
        // Kết nối database
        private readonly AppDbContext _context;

        public SettingService(AppDbContext context)
        {
            _context = context;
        }

        // =========================================
        // LẤY THÔNG TIN CÀI ĐẶT
        // =========================================

        public Setting GetSetting()
        {
            // Lấy setting đầu tiên trong database
            var setting = _context.Settings.FirstOrDefault();

            // Nếu chưa có dữ liệu
            // thì tạo setting mặc định
            if (setting == null)
            {
                setting = new Setting
                {
                    // Tên cửa hàng mặc định
                    StoreName = "",

                    // Email mặc định
                    StoreEmail = "escfood@gmail.com",

                    // SĐT mặc định
                    StorePhone = "0123 456 789",

                    // Địa chỉ mặc định
                    StoreAddress = "123 EscFood Street, Ha Noi City",

                    // Giờ mở cửa
                    OpenTime = "08:00",

                    // Giờ đóng cửa
                    CloseTime = "22:00",

                    // Logo mặc định
                    LogoUrl = "/images/Logo/LogoESC.",

                    // Phí ship mặc định
                    ShippingFee = 20000,

                    // Đơn tối thiểu
                    MinimumOrderAmount = 50000,

                    // Thời gian giao dự kiến
                    EstimatedDeliveryMinutes = 30,

                    // Bật COD
                    IsCodEnabled = true,

                    // Tắt chuyển khoản
                    IsBankTransferEnabled = false
                };

                // Thêm vào database
                _context.Settings.Add(setting);

                // Lưu database
                _context.SaveChanges();
            }

            return setting;
        }

        // =========================================
        // CẬP NHẬT CÀI ĐẶT
        // =========================================

        public void UpdateSetting(Setting model)
        {
            // Lấy setting hiện tại
            var setting = GetSetting();

            // =========================================
            // THÔNG TIN CỬA HÀNG
            // =========================================

            setting.StoreName = model.StoreName;

            setting.StoreEmail = model.StoreEmail;

            setting.StorePhone = model.StorePhone;

            setting.StoreAddress = model.StoreAddress;

            setting.OpenTime = model.OpenTime;

            setting.CloseTime = model.CloseTime;

            // =========================================
            // LOGO
            // =========================================

            // Nếu có upload logo mới
            // thì cập nhật logo
            if (!string.IsNullOrEmpty(model.LogoUrl))
            {
                setting.LogoUrl = model.LogoUrl;
            }

            // =========================================
            // CÀI ĐẶT ĐƠN HÀNG
            // =========================================

            // Phí ship
            setting.ShippingFee = model.ShippingFee;

            // Đơn tối thiểu
            setting.MinimumOrderAmount =
                model.MinimumOrderAmount;

            // Thời gian giao hàng
            setting.EstimatedDeliveryMinutes =
                model.EstimatedDeliveryMinutes;

            // =========================================
            // THANH TOÁN
            // =========================================

            // COD
            setting.IsCodEnabled =
                model.IsCodEnabled;

            // Chuyển khoản
            setting.IsBankTransferEnabled =
                model.IsBankTransferEnabled;

            // =========================================
            // LƯU DATABASE
            // =========================================

            _context.SaveChanges();
        }
    }
}