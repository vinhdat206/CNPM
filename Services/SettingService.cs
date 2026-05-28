using CNPMFastFood.Data;
using CNPMFastFood.Models;

namespace CNPMFastFood.Services
{
    // =========================================================
    // SETTING SERVICE
    // ---------------------------------------------------------
    // Service này dùng để quản lý thông tin cài đặt hệ thống.
    //
    // Các thông tin cài đặt gồm:
    // - Tên cửa hàng
    // - Email cửa hàng
    // - Số điện thoại
    // - Địa chỉ
    // - Giờ mở cửa / đóng cửa
    // - Logo
    // - Phí vận chuyển
    // - Giá trị đơn hàng tối thiểu
    // - Thời gian giao hàng dự kiến
    // - Phương thức thanh toán
    // =========================================================
    public class SettingService
    {
        // =====================================================
        // AppDbContext dùng để kết nối và thao tác với database
        // =====================================================
        private readonly AppDbContext _context;

        // =====================================================
        // CONSTRUCTOR
        // -----------------------------------------------------
        // ASP.NET Core sẽ tự động truyền AppDbContext vào đây
        // thông qua Dependency Injection
        // =====================================================
        public SettingService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // LẤY THÔNG TIN CÀI ĐẶT
        // =====================================================

        public Setting GetSetting()
        {
            // -------------------------------------------------
            // Lấy bản ghi setting đầu tiên trong database
            // Vì hệ thống thường chỉ cần 1 bản ghi cấu hình
            // -------------------------------------------------
            var setting = _context.Settings.FirstOrDefault();

            // -------------------------------------------------
            // Nếu chưa có bản ghi setting nào trong database
            // thì tạo dữ liệu mặc định
            // -------------------------------------------------
            if (setting == null)
            {
                setting = new Setting
                {
                    // Tên cửa hàng mặc định
                    StoreName = "",

                    // Email cửa hàng mặc định
                    StoreEmail = "escfood@gmail.com",

                    // Số điện thoại cửa hàng mặc định
                    StorePhone = "0123 456 789",

                    // Địa chỉ cửa hàng mặc định
                    StoreAddress = "123 EscFood Street, Ha Noi City",

                    // Giờ mở cửa
                    OpenTime = "08:00",

                    // Giờ đóng cửa
                    CloseTime = "22:00",

                    // Đường dẫn logo mặc định
                    LogoUrl = "/images/Logo/LogoESC.",

                    // Phí vận chuyển mặc định
                    ShippingFee = 20000,

                    // Giá trị đơn hàng tối thiểu
                    MinimumOrderAmount = 50000,

                    // Thời gian giao hàng dự kiến, tính bằng phút
                    EstimatedDeliveryMinutes = 30,

                    // Cho phép thanh toán khi nhận hàng
                    IsCodEnabled = true,

                    // Chưa bật thanh toán chuyển khoản
                    IsBankTransferEnabled = false
                };

                // Thêm setting mặc định vào database
                _context.Settings.Add(setting);

                // Lưu thay đổi xuống database
                _context.SaveChanges();
            }

            // Trả về setting hiện tại
            return setting;
        }

        // =====================================================
        // CẬP NHẬT CÀI ĐẶT
        // =====================================================

        public void UpdateSetting(Setting model)
        {
            // -------------------------------------------------
            // Lấy setting hiện tại.
            // Nếu chưa có thì GetSetting() sẽ tự tạo mặc định.
            // -------------------------------------------------
            var setting = GetSetting();

            // =================================================
            // CẬP NHẬT THÔNG TIN CỬA HÀNG
            // =================================================

            // Cập nhật tên cửa hàng
            setting.StoreName = model.StoreName;

            // Cập nhật email cửa hàng
            setting.StoreEmail = model.StoreEmail;

            // Cập nhật số điện thoại cửa hàng
            setting.StorePhone = model.StorePhone;

            // Cập nhật địa chỉ cửa hàng
            setting.StoreAddress = model.StoreAddress;

            // Cập nhật giờ mở cửa
            setting.OpenTime = model.OpenTime;

            // Cập nhật giờ đóng cửa
            setting.CloseTime = model.CloseTime;

            // =================================================
            // CẬP NHẬT LOGO
            // =================================================

            // Nếu model.LogoUrl không rỗng
            // nghĩa là người dùng đã upload hoặc chọn logo mới
            if (!string.IsNullOrEmpty(model.LogoUrl))
            {
                // Cập nhật đường dẫn logo mới
                setting.LogoUrl = model.LogoUrl;
            }

            // =================================================
            // CẬP NHẬT CÀI ĐẶT ĐƠN HÀNG
            // =================================================

            // Cập nhật phí vận chuyển
            setting.ShippingFee = model.ShippingFee;

            // Cập nhật giá trị đơn hàng tối thiểu
            setting.MinimumOrderAmount =
                model.MinimumOrderAmount;

            // Cập nhật thời gian giao hàng dự kiến
            setting.EstimatedDeliveryMinutes =
                model.EstimatedDeliveryMinutes;

            // =================================================
            // CẬP NHẬT PHƯƠNG THỨC THANH TOÁN
            // =================================================

            // Cập nhật trạng thái thanh toán COD
            setting.IsCodEnabled =
                model.IsCodEnabled;

            // Cập nhật trạng thái thanh toán chuyển khoản
            setting.IsBankTransferEnabled =
                model.IsBankTransferEnabled;

            // =================================================
            // LƯU DATABASE
            // =================================================

            // Lưu toàn bộ thay đổi xuống database
            _context.SaveChanges();
        }
    }
}
