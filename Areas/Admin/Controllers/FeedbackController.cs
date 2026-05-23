using CNPMFastFood.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeedbackController : Controller
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Feedback";
            ViewData["PageTitle"] = "Quản lý feedback";

            var products = await _context.Products
                .Include(p => p.Reviews)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                TempData["Error"] = "Không tìm thấy feedback!";
                return RedirectToAction(nameof(Index));
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xóa feedback!";

            return RedirectToAction(nameof(Index));
        }
    }
}