using CNPMFastFood.Data;

namespace CNPMFastFood.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public decimal GetTotalRevenue()
        {
            return 125000000;
        }

        public int GetTotalOrders()
        {
            return 1284;
        }

        public int GetTotalCustomers()
        {
            return 562;
        }

        public List<decimal> GetMonthlyRevenue()
        {
            return new List<decimal>
            {
                12000000,
                25000000,
                18000000,
                32000000,
                41000000,
                39000000,
                50000000,
                47000000,
                62000000,
                70000000,
                82000000,
                95000000
            };
        }
    }
}