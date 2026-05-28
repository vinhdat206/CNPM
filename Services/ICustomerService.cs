using CNPMFastFood.Models;

namespace CNPMFastFood.Services
{
    // =========================================================
    // INTERFACE QUẢN LÝ KHÁCH HÀNG
    // ---------------------------------------------------------
    // Interface này định nghĩa các chức năng bắt buộc
    // mà CustomerService phải triển khai.
    //
    // Interface chỉ khai báo hàm, không viết phần xử lý bên trong.
    // Phần xử lý thật sẽ nằm trong class CustomerService.
    // =========================================================
    public interface ICustomerService
    {
        // =====================================================
        // LẤY DANH SÁCH USER
        // -----------------------------------------------------
        // Trả về danh sách tất cả user trong hệ thống.
        //
        // Tham số:
        // keyword: từ khóa tìm kiếm
        //
        // Có thể tìm theo:
        // - Username
        // - Email
        // - Role
        //
        // Task<List<User>>:
        // Hàm chạy bất đồng bộ và trả về danh sách User.
        // =====================================================
        Task<List<User>> GetAllCustomersAsync(
            string keyword);

        // =====================================================
        // LẤY CHI TIẾT USER THEO ID
        // -----------------------------------------------------
        // Dùng để lấy thông tin chi tiết của một user cụ thể.
        //
        // Tham số:
        // id: mã user cần tìm
        //
        // User?:
        // Có thể trả về User nếu tìm thấy,
        // hoặc null nếu không tồn tại.
        // =====================================================
        Task<User?> GetCustomerByIdAsync(
            int id);

        // =====================================================
        // XÓA USER
        // -----------------------------------------------------
        // Xóa tài khoản user khỏi database.
        //
        // Trả về:
        // true  -> xóa thành công
        // false -> không tìm thấy user hoặc xóa thất bại
        // =====================================================
        Task<bool> DeleteCustomerAsync(
            int id);

        // =====================================================
        // CẬP NHẬT ROLE
        // -----------------------------------------------------
        // Cập nhật quyền của user.
        //
        // Tham số:
        // id   : mã user
        // role : quyền mới
        //
        // Ví dụ role:
        // - Admin
        // - Staff
        // - Customer
        // =====================================================
        Task<bool> UpdateRoleAsync(
            int id,
            string role);

        // =====================================================
        // KHÓA TÀI KHOẢN
        // -----------------------------------------------------
        // Khóa tài khoản user.
        //
        // Khi bị khóa, user có thể không được đăng nhập
        // hoặc không được sử dụng hệ thống.
        // =====================================================
        Task<bool> BlockUserAsync(
            int id);

        // =====================================================
        // MỞ KHÓA TÀI KHOẢN
        // -----------------------------------------------------
        // Mở khóa tài khoản user đã bị khóa trước đó.
        //
        // Sau khi mở khóa, user có thể đăng nhập lại bình thường.
        // =====================================================
        Task<bool> UnblockUserAsync(
            int id);

        // =====================================================
        // RESET PASSWORD
        // -----------------------------------------------------
        // Đặt lại mật khẩu mới cho user.
        //
        // Tham số:
        // id          : mã user
        // newPassword : mật khẩu mới
        //
        // Việc mã hóa mật khẩu sẽ được xử lý
        // trong CustomerService.
        // =====================================================
        Task<bool> ResetPasswordAsync(
            int id,
            string newPassword);
    }
}