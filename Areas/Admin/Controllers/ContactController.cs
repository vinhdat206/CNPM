using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNPMFastFood.Data;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách liên hệ
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Liên hệ";
            ViewData["PageTitle"] = "Danh sách liên hệ";

            var contacts = await _context.ContactMessages
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(contacts);
        }

        // Đánh dấu đã đọc
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var contact = await _context.ContactMessages.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            contact.IsRead = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Xóa liên hệ
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.ContactMessages.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            _context.ContactMessages.Remove(contact);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}