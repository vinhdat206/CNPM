using CNPMFastFood.Data;
using CNPMFastFood.Models;
using Microsoft.EntityFrameworkCore;

namespace CNPMFastFood.Services
{
    // =========================================================
    // SERVICE QUẢN LÝ KHÁCH HÀNG (CUSTOMER SERVICE)
    // ---------------------------------------------------------
    // Class này dùng để xử lý toàn bộ nghiệp vụ liên quan đến User:
    // - Lấy danh sách khách hàng
    // - Xem chi tiết khách hàng
    // - Xóa tài khoản
    // - Cập nhật quyền (Role)
    // - Khóa / mở khóa tài khoản
    // - Reset mật khẩu
    //
    // Service sẽ làm việc trực tiếp với Database thông qua AppDbContext
    // =========================================================
    public class CustomerService : ICustomerService
    {
        // =====================================================
        // BIẾN _context
        // -----------------------------------------------------
        // AppDbContext là lớp kết nối tới SQL Server bằng EF Core
        // _context sẽ cho phép truy cập các bảng trong database
        //
        // Ví dụ:
        // _context.Users -> bảng Users
        // =====================================================
        private readonly AppDbContext _context;

        // =====================================================
        // CONSTRUCTOR
        // -----------------------------------------------------
        // Dependency Injection:
        // ASP.NET Core sẽ tự động truyền AppDbContext vào đây
        // khi CustomerService được khởi tạo
        // =====================================================
        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // LẤY DANH SÁCH USER
        // =====================================================

        public async Task<List<User>> GetAllCustomersAsync(
            string keyword)
        {
            var query = _context.Users
                .Where(u => u.Role != "admin")
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>

                    u.Username.Contains(keyword)

                    || u.Email.Contains(keyword)

                    || u.Role.Contains(keyword)
                );
            }

            return await query
                .OrderByDescending(u => u.Id)
                .ToListAsync();
        }

        // =====================================================
        // LẤY CHI TIẾT USER THEO ID
        // =====================================================

        public async Task<User?> GetCustomerByIdAsync(int id)
        {
            // -------------------------------------------------
            // FirstOrDefaultAsync:
            // - Tìm user đầu tiên thỏa điều kiện
            // - Nếu không có => trả về null
            // -------------------------------------------------
            return await _context.Users

                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // =====================================================
        // XÓA USER
        // =====================================================

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            // -------------------------------------------------
            // Tìm user theo khóa chính (Primary Key)
            // FindAsync chạy nhanh hơn FirstOrDefaultAsync
            // khi tìm bằng Id
            // -------------------------------------------------
            var user = await _context.Users.FindAsync(id);

            // -------------------------------------------------
            // Nếu không tìm thấy user
            // -------------------------------------------------
            if (user == null)
            {
                return false;
            }

            // -------------------------------------------------
            // Remove() -> đánh dấu entity để xóa
            // -------------------------------------------------
            _context.Users.Remove(user);

            // -------------------------------------------------
            // SaveChangesAsync() -> lưu thay đổi xuống database
            // -------------------------------------------------
            await _context.SaveChangesAsync();

            return true;
        }

        // =====================================================
        // CẬP NHẬT ROLE
        // =====================================================

        public async Task<bool> UpdateRoleAsync(
            int id,
            string role)
        {
            // -------------------------------------------------
            // Tìm user theo Id
            // -------------------------------------------------
            var user = await _context.Users.FindAsync(id);

            // -------------------------------------------------
            // Nếu không tồn tại
            // -------------------------------------------------
            if (user == null)
            {
                return false;
            }

            // -------------------------------------------------
            // Gán role mới
            //
            // Ví dụ:
            // "Admin"
            // "Customer"
            // "Staff"
            // -------------------------------------------------
            user.Role = role;

            // -------------------------------------------------
            // Lưu thay đổi vào database
            // -------------------------------------------------
            await _context.SaveChangesAsync();

            return true;
        }

        // =====================================================
        // KHÓA TÀI KHOẢN
        // =====================================================

        public async Task<bool> BlockUserAsync(int id)
        {
            // -------------------------------------------------
            // Tìm user
            // -------------------------------------------------
            var user = await _context.Users.FindAsync(id);

            // -------------------------------------------------
            // Nếu không tồn tại
            // -------------------------------------------------
            if (user == null)
            {
                return false;
            }

            // -------------------------------------------------
            // Đặt IsBlocked = true
            // User sẽ bị khóa đăng nhập
            // -------------------------------------------------
            user.IsBlocked = true;

            // -------------------------------------------------
            // Lưu thay đổi
            // -------------------------------------------------
            await _context.SaveChangesAsync();

            return true;
        }

        // =====================================================
        // MỞ KHÓA TÀI KHOẢN
        // =====================================================

        public async Task<bool> UnblockUserAsync(int id)
        {
            // -------------------------------------------------
            // Tìm user
            // -------------------------------------------------
            var user = await _context.Users.FindAsync(id);

            // -------------------------------------------------
            // Nếu không tồn tại
            // -------------------------------------------------
            if (user == null)
            {
                return false;
            }

            // -------------------------------------------------
            // Mở khóa tài khoản
            // -------------------------------------------------
            user.IsBlocked = false;

            // -------------------------------------------------
            // Lưu database
            // -------------------------------------------------
            await _context.SaveChangesAsync();

            return true;
        }

        // =====================================================
        // RESET PASSWORD
        // =====================================================

        public async Task<bool> ResetPasswordAsync(
            int id,
            string newPassword)
        {
            // -------------------------------------------------
            // Tìm user
            // -------------------------------------------------
            var user = await _context.Users.FindAsync(id);

            // -------------------------------------------------
            // Nếu không tồn tại
            // -------------------------------------------------
            if (user == null)
            {
                return false;
            }

            // -------------------------------------------------
            // BCrypt.HashPassword():
            // Mã hóa mật khẩu trước khi lưu database
            //
            // KHÔNG BAO GIỜ lưu mật khẩu dạng plain text
            // vì sẽ rất nguy hiểm
            // -------------------------------------------------
            user.Password =
                BCrypt.Net.BCrypt.HashPassword(newPassword);

            // -------------------------------------------------
            // Lưu thay đổi xuống database
            // -------------------------------------------------
            await _context.SaveChangesAsync();

            return true;
        }
    }
}