using CNPMFastFood.Data;

namespace CNPMFastFood.Services
{
    // =========================================================
    // DASHBOARD SERVICE
    // ---------------------------------------------------------
    // Service này dùng để xử lý dữ liệu thống kê cho Dashboard
    //
    // Chức năng:
    // - Tổng doanh thu
    // - Tổng số đơn hàng
    // - Tổng số khách hàng
    // - Doanh thu theo tháng
    //
    // DashboardService sẽ lấy dữ liệu từ Database
    // thông qua AppDbContext
    // =========================================================
    public class DashboardService
    {
        // =====================================================
        // BIẾN _context
        // -----------------------------------------------------
        // AppDbContext là lớp kết nối tới database SQL Server
        //
        // Thông qua _context ta có thể truy cập:
        // - Orders
        // - Users
        // - Products
        // - ...
        // =====================================================
        private readonly AppDbContext _context;

        // =====================================================
        // CONSTRUCTOR
        // -----------------------------------------------------
        // Dependency Injection:
        // ASP.NET Core sẽ tự động inject AppDbContext
        // khi DashboardService được khởi tạo
        // =====================================================
        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // LẤY TỔNG DOANH THU
        // =====================================================

        public decimal GetTotalRevenue()
        {
            // -------------------------------------------------
            // decimal:
            // Kiểu dữ liệu dùng cho tiền tệ
            // chính xác hơn float/double
            // -------------------------------------------------

            // -------------------------------------------------
            // Hiện tại đang trả về dữ liệu giả (hard-code)
            // -------------------------------------------------
            return 125000000;

            // -------------------------------------------------
            // Sau này có thể thay bằng:
            //
            // return _context.Orders
            //     .Where(o => o.Status == "Completed")
            //     .Sum(o => o.TotalPrice);
            // -------------------------------------------------
        }

        // =====================================================
        // LẤY TỔNG SỐ ĐƠN HÀNG
        // =====================================================

        public int GetTotalOrders()
        {
            // -------------------------------------------------
            // Trả về tổng số đơn hàng
            // Hiện tại là dữ liệu mẫu
            // -------------------------------------------------
            return 1284;

            // -------------------------------------------------
            // Sau này có thể dùng:
            //
            // return _context.Orders.Count();
            // -------------------------------------------------
        }

        // =====================================================
        // LẤY TỔNG SỐ KHÁCH HÀNG
        // =====================================================

        public int GetTotalCustomers()
        {
            // -------------------------------------------------
            // Trả về số lượng khách hàng
            // Hiện tại là dữ liệu demo
            // -------------------------------------------------
            return 562;

            // -------------------------------------------------
            // Sau này có thể thay bằng:
            //
            // return _context.Users
            //     .Count(u => u.Role == "Customer");
            // -------------------------------------------------
        }

        // =====================================================
        // LẤY DOANH THU THEO TỪNG THÁNG
        // =====================================================

        public List<decimal> GetMonthlyRevenue()
        {
            // -------------------------------------------------
            // List<decimal>:
            // Danh sách doanh thu từng tháng
            //
            // Index:
            // [0]  = Tháng 1
            // [1]  = Tháng 2
            // ...
            // [11] = Tháng 12
            // -------------------------------------------------

            return new List<decimal>
            {
                // Tháng 1
                12000000,

                // Tháng 2
                25000000,

                // Tháng 3
                18000000,

                // Tháng 4
                32000000,

                // Tháng 5
                41000000,

                // Tháng 6
                39000000,

                // Tháng 7
                50000000,

                // Tháng 8
                47000000,

                // Tháng 9
                62000000,

                // Tháng 10
                70000000,

                // Tháng 11
                82000000,

                // Tháng 12
                95000000
            };

            // -------------------------------------------------
            // Sau này có thể lấy từ database:
            //
            // return _context.Orders
            //     .Where(o => o.Status == "Completed")
            //     .GroupBy(o => o.CreatedAt.Month)
            //     .Select(g => g.Sum(o => o.TotalPrice))
            //     .ToList();
            // -------------------------------------------------
        }
    }
}