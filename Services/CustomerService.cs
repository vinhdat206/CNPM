using CNPMFastFood.Data;
using CNPMFastFood.Models;
using Microsoft.EntityFrameworkCore;

namespace CNPMFastFood.Services
{
    // Service xử lý dữ liệu khách hàng
    // Chứa toàn bộ nghiệp vụ liên quan đến User
    public class CustomerService : ICustomerService
    {
        // DbContext kết nối SQL Server
        private readonly AppDbContext _context;

        // Inject AppDbContext
        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // LẤY DANH SÁCH USER
        // =========================

        public async Task<List<User>> GetAllCustomersAsync(
            string keyword)
        {
            // Query bảng Users
            var query = _context.Users.AsQueryable();

            // Nếu có keyword thì tìm kiếm
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>

                    // Tìm theo username
                    u.Username.Contains(keyword)

                    // Hoặc email
                    || u.Email.Contains(keyword)

                    // Hoặc role
                    || u.Role.Contains(keyword)
                );
            }

            // Sắp xếp user mới nhất lên đầu
            return await query
                .OrderByDescending(u => u.Id)
                .ToListAsync();
        }

        // =========================
        // LẤY CHI TIẾT USER
        // =========================

        public async Task<User?> GetCustomerByIdAsync(int id)
        {
            // Tìm user theo Id
            return await _context.Users

                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // =========================
        // XÓA USER
        // =========================

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            // Tìm user
            var user = await _context.Users.FindAsync(id);

            // Nếu không tồn tại
            if (user == null)
            {
                return false;
            }

            // Xóa user khỏi database
            _context.Users.Remove(user);

            // Lưu thay đổi
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // CẬP NHẬT ROLE
        // =========================

        public async Task<bool> UpdateRoleAsync(
            int id,
            string role)
        {
            // Tìm user
            var user = await _context.Users.FindAsync(id);

            // Nếu không tồn tại
            if (user == null)
            {
                return false;
            }

            // Cập nhật role
            user.Role = role;

            // Lưu database
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // KHÓA TÀI KHOẢN
        // =========================

        public async Task<bool> BlockUserAsync(int id)
        {
            // Tìm user
            var user = await _context.Users.FindAsync(id);

            // Nếu không tồn tại
            if (user == null)
            {
                return false;
            }

            // Khóa tài khoản
            user.IsBlocked = true;

            // Lưu database
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // MỞ KHÓA TÀI KHOẢN
        // =========================

        public async Task<bool> UnblockUserAsync(int id)
        {
            // Tìm user
            var user = await _context.Users.FindAsync(id);

            // Nếu không tồn tại
            if (user == null)
            {
                return false;
            }

            // Mở khóa tài khoản
            user.IsBlocked = false;

            // Lưu database
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // RESET PASSWORD
        // =========================

        public async Task<bool> ResetPasswordAsync(
            int id,
            string newPassword)
        {
            // Tìm user
            var user = await _context.Users.FindAsync(id);

            // Nếu không tồn tại
            if (user == null)
            {
                return false;
            }

            // Mã hóa password bằng BCrypt
            user.Password =
                BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Lưu database
            await _context.SaveChangesAsync();

            return true;
        }
    }
}