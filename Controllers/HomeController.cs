// File: Controllers/HomeController.cs
// Mô tả:
// Điều hướng trang chủ

using Microsoft.AspNetCore.Mvc;

using CNPMFastFood.Services;

namespace CNPMFastFood.Controllers
{
    public class HomeController : Controller
    {
        // Service sản phẩm
        private readonly ProductService
            _productService;

        // Constructor
        public HomeController(
            ProductService productService)
        {
            _productService =
                productService;
        }

        // =========================
        // HOME PAGE
        // =========================

        public IActionResult Index()
        {
            // lấy danh sách sản phẩm
            var products =
                _productService.GetAll();

            // truyền sang view
            return View(products);
        }
    }
}