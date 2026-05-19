// File: Controllers/MenuController.cs
// Mô tả:
// Controller hiển thị trang sản phẩm

using Microsoft.AspNetCore.Mvc;

using CNPMFastFood.Services;

namespace CNPMFastFood.Controllers
{
    public class MenuController : Controller
    {
        // product service
        private readonly ProductService
            _productService;

        // constructor inject service
        public MenuController(
            ProductService productService)
        {
            _productService =
                productService;
        }

        // =========================
        // PRODUCT PAGE
        // =========================

        public IActionResult Index()
        {
            // gọi service lấy data
            var products =
                _productService.GetAll();

            // truyền view
            return View(products);
        }
    }
}