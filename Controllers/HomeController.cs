// File: Controllers/HomeController.cs
// Mô tả:
// Điều hướng trang chủ, trang giới thiệu, trang liên hệ

using Microsoft.AspNetCore.Mvc;
using CNPMFastFood.Services;
using CNPMFastFood.Models;
using CNPMFastFood.Data;

namespace CNPMFastFood.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService;

        private readonly AppDbContext _context;

        public HomeController(
            ProductService productService,
            AppDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        // =========================
        // HOME PAGE
        // =========================

        public IActionResult Index()
        {
            var products = _productService.GetAll()
                .Where(p => p.Featured == true)
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(products);
        }

        // =========================
        // ABOUT PAGE
        // =========================

        public IActionResult About()
        {
            return View();
        }

        // =========================
        // CONTACT PAGE - GET
        // Hiển thị form liên hệ
        // =========================

        [HttpGet]
        public IActionResult Contact()
        {
            return View(new ContactMessage());
        }

        // =========================
        // CONTACT PAGE - POST
        // Nhận dữ liệu user gửi và lưu database
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMessage model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedAt = DateTime.Now;
            model.IsRead = false;

            _context.ContactMessages.Add(model);

            await _context.SaveChangesAsync();

            ViewBag.Success = "Gửi liên hệ thành công!";

            ModelState.Clear();

            return View(new ContactMessage());
        }

        // =========================
        // POLICY PAGE
        // =========================

        public IActionResult Policy()
        {
            return View();
        }
    }
}