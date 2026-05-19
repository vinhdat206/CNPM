using System;
using System.ComponentModel.DataAnnotations;

namespace CNPMFastFood.Models
{
    public class ContactMessage
    {
        // Khóa chính của bảng
        public int Id { get; set; }

        // Tên người gửi
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Name { get; set; }

        // Email người gửi
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        // Số điện thoại người gửi
        public string Phone { get; set; }

        // Nội dung liên hệ
        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Message { get; set; }

        // Ngày gửi liên hệ
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Trạng thái admin đã đọc hay chưa
        public bool IsRead { get; set; } = false;
    }
}