using System.ComponentModel.DataAnnotations.Schema;

namespace CNPMFastFood.Models
{
    public class User
    {
        // Id người dùng
        public int Id { get; set; }

        // Tên đăng nhập
        public string Username { get; set; }

        // Email
        public string Email { get; set; }

        // Mật khẩu
        public string Password { get; set; }

        // Xác nhận mật khẩu
        // Không lưu database
        [NotMapped]
        public string ConfirmPassword { get; set; }

        // Vai trò
        // admin hoặc user
        public string Role { get; set; }
        
        // Khóa / mở khóa tài khoản
        public bool IsBlocked { get; set; } = false;

        // Bắt user đổi mật khẩu sau khi đăng nhập
        public bool ForceChangePassword { get; set; } = false;
    }
}