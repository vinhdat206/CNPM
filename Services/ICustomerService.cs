using CNPMFastFood.Models;

namespace CNPMFastFood.Services
{
    // Interface định nghĩa các chức năng
    // quản lý tài khoản khách hàng
    public interface ICustomerService
    {
        // =========================
        // LẤY DANH SÁCH USER
        // =========================
        // Có hỗ trợ tìm kiếm keyword

        Task<List<User>> GetAllCustomersAsync(
            string keyword);

        // =========================
        // LẤY CHI TIẾT USER
        // =========================

        Task<User?> GetCustomerByIdAsync(
            int id);

        // =========================
        // XÓA USER
        // =========================

        Task<bool> DeleteCustomerAsync(
            int id);

        // =========================
        // CẬP NHẬT ROLE
        // =========================

        Task<bool> UpdateRoleAsync(
            int id,
            string role);

        // =========================
        // KHÓA TÀI KHOẢN
        // =========================

        Task<bool> BlockUserAsync(
            int id);

        // =========================
        // MỞ KHÓA TÀI KHOẢN
        // =========================

        Task<bool> UnblockUserAsync(
            int id);

        // =========================
        // RESET PASSWORD
        // =========================

        Task<bool> ResetPasswordAsync(
            int id,
            string newPassword);
    }
}