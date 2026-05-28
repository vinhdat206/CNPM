// =============================
// FILE: Services/ReportService.cs
// =============================

using CNPMFastFood.Data;

namespace CNPMFastFood.Services
{
    // =========================================================
    // REPORT SERVICE
    // ---------------------------------------------------------
    // Service này dùng để xử lý dữ liệu báo cáo / thống kê.
    //
    // Các báo cáo gồm:
    // - Top sản phẩm bán chạy
    // - Số lượng đơn hàng theo trạng thái
    // - Doanh thu theo sản phẩm
    // - Top khách hàng mua nhiều
    //
    // Hiện tại dữ liệu đang là dữ liệu mẫu hard-code.
    // Sau này có thể thay bằng dữ liệu lấy từ database.
    // =========================================================
    public class ReportService
    {
        // =====================================================
        // AppDbContext dùng để kết nối và truy vấn database
        // =====================================================
        private readonly AppDbContext _context;

        // =====================================================
        // CONSTRUCTOR
        // -----------------------------------------------------
        // ASP.NET Core sẽ tự động inject AppDbContext vào service
        // =====================================================
        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // TOP SẢN PHẨM BÁN CHẠY
        // =====================================================

        // Lấy danh sách tên các sản phẩm bán chạy nhất
        public List<string> GetTopProductNames()
        {
            return new List<string>
            {
                "Burger bò",
                "Gà rán",
                "Pizza hải sản",
                "Khoai tây chiên",
                "Trà sữa"
            };
        }

        // Lấy số lượng bán tương ứng với từng sản phẩm ở trên
        public List<int> GetTopProductSales()
        {
            return new List<int>
            {
                // Burger bò bán được 120
                120,

                // Gà rán bán được 95
                95,

                // Pizza hải sản bán được 80
                80,

                // Khoai tây chiên bán được 65
                65,

                // Trà sữa bán được 50
                50
            };
        }

        // =====================================================
        // ĐƠN HÀNG THEO TRẠNG THÁI
        // =====================================================

        // Lấy danh sách nhãn trạng thái đơn hàng
        public List<string> GetOrderStatusLabels()
        {
            return new List<string>
            {
                "Hoàn thành",
                "Đang xử lý",
                "Đã hủy"
            };
        }

        // Lấy số lượng đơn hàng tương ứng với từng trạng thái
        public List<int> GetOrderStatusValues()
        {
            return new List<int>
            {
                // Số đơn hoàn thành
                280,

                // Số đơn đang xử lý
                45,

                // Số đơn đã hủy
                25
            };
        }

        // =====================================================
        // DOANH THU THEO SẢN PHẨM
        // =====================================================

        // Lấy danh sách tên sản phẩm để hiển thị trong biểu đồ doanh thu
        public List<string> GetProductRevenueNames()
        {
            return new List<string>
            {
                "Burger bò",
                "Gà rán",
                "Pizza hải sản",
                "Khoai tây chiên",
                "Trà sữa"
            };
        }

        // Lấy doanh thu tương ứng của từng sản phẩm
        public List<decimal> GetProductRevenueValues()
        {
            return new List<decimal>
            {
                // Doanh thu Burger bò
                18000000,

                // Doanh thu Gà rán
                15000000,

                // Doanh thu Pizza hải sản
                13500000,

                // Doanh thu Khoai tây chiên
                8000000,

                // Doanh thu Trà sữa
                6000000
            };
        }

        // =====================================================
        // TOP KHÁCH HÀNG
        // =====================================================

        // Lấy danh sách khách hàng mua nhiều nhất
        public List<dynamic> GetTopCustomers()
        {
            return new List<dynamic>
            {
                new
                {
                    // Tên khách hàng
                    Name = "Nguyễn Văn A",

                    // Tổng số đơn đã mua
                    Orders = 12,

                    // Tổng tiền đã chi tiêu
                    TotalSpent = 2500000
                },

                new
                {
                    Name = "Trần Thị B",
                    Orders = 9,
                    TotalSpent = 1800000
                },

                new
                {
                    Name = "Lê Văn C",
                    Orders = 7,
                    TotalSpent = 1350000
                }
            };
        }
    }
}