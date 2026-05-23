using CNPMFastFood.Data;
using CNPMFastFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPMFastFood.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int orderId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                TempData["Error"] = "Số sao không hợp lệ!";
                return RedirectToAction("History", "Order");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                TempData["Error"] = "Vui lòng nhập nội dung đánh giá!";
                return RedirectToAction("History", "Order");
            }

            var userName = User.Identity?.Name ?? "Khách hàng";

            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null || order.Status != "Completed")
            {
                TempData["Error"] = "Chỉ có thể đánh giá đơn hàng đã hoàn thành!";
                return RedirectToAction("History", "Order");
            }

            foreach (var item in order.OrderDetails)
            {
                var review = new Review
                {
                    UserName = userName,
                    Rating = rating,
                    Comment = comment,
                    ProductId = item.ProductId,
                    OrderId = orderId,
                    CreatedAt = DateTime.Now
                };

                _context.Reviews.Add(review);
            }

            _context.SaveChanges();

            TempData["Success"] = "Gửi đánh giá thành công!";

            return RedirectToAction("History", "Order");
        }
    }
}