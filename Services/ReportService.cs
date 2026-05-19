// =============================
// FILE: Services/ReportService.cs
// =============================

using CNPMFastFood.Data;

namespace CNPMFastFood.Services
{
    public class ReportService
    {
        // Database context
        private readonly AppDbContext _context;

        // Constructor
        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        // =============================
        // TOP SẢN PHẨM BÁN CHẠY
        // =============================

        // Tên sản phẩm
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

        // Số lượng bán
        public List<int> GetTopProductSales()
        {
            return new List<int>
            {
                120,
                95,
                80,
                65,
                50
            };
        }

        // =============================
        // ĐƠN HÀNG THEO TRẠNG THÁI
        // =============================

        // Tên trạng thái
        public List<string> GetOrderStatusLabels()
        {
            return new List<string>
            {
                "Hoàn thành",
                "Đang xử lý",
                "Đã hủy"
            };
        }

        // Số lượng theo trạng thái
        public List<int> GetOrderStatusValues()
        {
            return new List<int>
            {
                280,
                45,
                25
            };
        }

        // =============================
        // DOANH THU THEO SẢN PHẨM
        // =============================

        // Tên sản phẩm
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

        // Doanh thu từng sản phẩm
        public List<decimal> GetProductRevenueValues()
        {
            return new List<decimal>
            {
                18000000,
                15000000,
                13500000,
                8000000,
                6000000
            };
        }

        // =============================
        // TOP KHÁCH HÀNG
        // =============================

        public List<dynamic> GetTopCustomers()
        {
            return new List<dynamic>
            {
                new
                {
                    Name = "Nguyễn Văn A",
                    Orders = 12,
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