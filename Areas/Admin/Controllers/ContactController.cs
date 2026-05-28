// Import các thư viện cần thiết

// Thư viện hỗ trợ xây dựng Controller trong ASP.NET Core MVC
using Microsoft.AspNetCore.Mvc;

// Thư viện hỗ trợ thao tác bất đồng bộ với Entity Framework Core
using Microsoft.EntityFrameworkCore;

// Namespace chứa AppDbContext (kết nối Database)
using CNPMFastFood.Data;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // Đánh dấu Controller này thuộc Area "Admin"
    // URL sẽ có dạng: /Admin/Contact/Index
    [Area("Admin")]
    public class ContactController : Controller
    {
        // Khai báo biến _context để thao tác với database
        // readonly nghĩa là chỉ được gán giá trị một lần trong constructor
        private readonly AppDbContext _context;

        // Constructor của Controller
        // Dependency Injection sẽ tự động truyền AppDbContext vào
        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================
        // HIỂN THỊ DANH SÁCH LIÊN HỆ
        // ==========================

        // Action Index dùng để hiển thị toàn bộ liên hệ
        // async/await giúp xử lý bất đồng bộ, tránh block server
        public async Task<IActionResult> Index()
        {
            // Truyền dữ liệu sang View
            // Dùng để hiển thị tiêu đề trang
            ViewData["Title"] = "Liên hệ";

            // Tiêu đề chính của trang
            ViewData["PageTitle"] = "Danh sách liên hệ";

            // Lấy danh sách liên hệ từ database
            // _context.ContactMessages:
            //     bảng ContactMessages trong database

            // OrderByDescending(x => x.CreatedAt):
            //     sắp xếp theo ngày tạo giảm dần
            //     => liên hệ mới nhất sẽ hiển thị đầu tiên

            // ToListAsync():
            //     thực thi query và chuyển kết quả thành List
            var contacts = await _context.ContactMessages
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            // Trả dữ liệu sang View
            // View sẽ nhận model là contacts
            return View(contacts);
        }

        // ==========================
        // ĐÁNH DẤU ĐÃ ĐỌC
        // ==========================

        // id là mã của liên hệ cần cập nhật
        public async Task<IActionResult> MarkAsRead(int id)
        {
            // Tìm liên hệ theo khóa chính (Primary Key)
            // FindAsync nhanh hơn FirstOrDefaultAsync nếu tìm theo ID
            var contact = await _context.ContactMessages.FindAsync(id);

            // Nếu không tìm thấy liên hệ
            if (contact == null)
            {
                // Trả về lỗi 404
                return NotFound();
            }

            // Cập nhật trạng thái đã đọc
            // true = đã đọc
            contact.IsRead = true;

            // Lưu thay đổi xuống database
            await _context.SaveChangesAsync();

            // Sau khi cập nhật xong
            // chuyển hướng về trang danh sách
            return RedirectToAction("Index");
        }

        // ==========================
        // XÓA LIÊN HỆ
        // ==========================

        // id là mã liên hệ cần xóa
        public async Task<IActionResult> Delete(int id)
        {
            // Tìm liên hệ theo ID
            var contact = await _context.ContactMessages.FindAsync(id);

            // Nếu không tồn tại
            if (contact == null)
            {
                // Trả về lỗi 404
                return NotFound();
            }

            // Xóa đối tượng khỏi DbSet
            _context.ContactMessages.Remove(contact);

            // Lưu thay đổi xuống database
            await _context.SaveChangesAsync();

            // Quay lại trang danh sách sau khi xóa
            return RedirectToAction("Index");
        }
    }
}