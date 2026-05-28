// Import namespace chứa Model Product
using CNPMFastFood.Models;

// Import namespace chứa ProductService
using CNPMFastFood.Services;

// Import thư viện ASP.NET Core MVC
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    // =====================================================
    // Controller quản lý sản phẩm thuộc khu vực Admin
    // URL mặc định:
    // /Admin/Product/Index
    // =====================================================

    [Area("Admin")]
    public class ProductController : Controller
    {
        // =====================================================
        // Service xử lý nghiệp vụ sản phẩm
        // =====================================================

        private readonly ProductService _productService;

        // =====================================================
        // Constructor
        // ASP.NET Core sẽ tự động inject ProductService
        // thông qua Dependency Injection
        // =====================================================

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // =====================================================
        // HIỂN THỊ DANH SÁCH SẢN PHẨM
        // =====================================================

        // GET: /Admin/Product
        public IActionResult Index()
        {
            // =================================================
            // Gọi service lấy toàn bộ sản phẩm
            // =================================================

            var products = _productService.GetAll();

            // =================================================
            // Trả danh sách sản phẩm sang View Index.cshtml
            // =================================================

            return View(products);
        }

        // =====================================================
        // HIỂN THỊ FORM THÊM SẢN PHẨM
        // =====================================================

        // GET: /Admin/Product/Create
        [HttpGet]
        public IActionResult Create()
        {
            // =================================================
            // Chỉ hiển thị form Create.cshtml
            // =================================================

            return View();
        }

        // =====================================================
        // XỬ LÝ THÊM SẢN PHẨM
        // =====================================================

        // POST: /Admin/Product/Create
        [HttpPost]

        // Chống tấn công CSRF
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            // =================================================
            // Kiểm tra dữ liệu nhập vào có hợp lệ không
            //
            // ModelState.IsValid sẽ kiểm tra:
            // - Required
            // - StringLength
            // - Range
            // - DataAnnotation trong Model
            // =================================================

            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ
                // trả lại form cùng dữ liệu đã nhập
                return View(product);
            }

            // =================================================
            // Gọi service thêm sản phẩm vào database
            // =================================================

            _productService.Add(product);

            // =================================================
            // Sau khi thêm thành công
            // chuyển về trang danh sách sản phẩm
            // =================================================

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // HIỂN THỊ FORM SỬA SẢN PHẨM
        // =====================================================

        // GET: /Admin/Product/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // =================================================
            // Tìm sản phẩm theo ID
            // =================================================

            var product = _productService.GetById(id);

            // =================================================
            // Nếu không tìm thấy sản phẩm
            // trả về lỗi 404
            // =================================================

            if (product == null)
            {
                return NotFound();
            }

            // =================================================
            // Trả dữ liệu sản phẩm sang form Edit.cshtml
            // =================================================

            return View(product);
        }

        // =====================================================
        // XỬ LÝ CẬP NHẬT SẢN PHẨM
        // =====================================================

        // POST: /Admin/Product/Edit
        [HttpPost]

        // Chống CSRF Attack
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            // =================================================
            // Kiểm tra dữ liệu nhập vào hợp lệ không
            // =================================================

            if (!ModelState.IsValid)
            {
                // Nếu lỗi validation
                // trả lại form cùng dữ liệu người dùng nhập
                return View(product);
            }

            // =================================================
            // Gọi service cập nhật sản phẩm
            // =================================================

            _productService.Update(product);

            // =================================================
            // Sau khi cập nhật xong
            // quay lại danh sách sản phẩm
            // =================================================

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // XÓA SẢN PHẨM
        // =====================================================

        // POST: /Admin/Product/Delete/5
        [HttpPost]

        // Chống CSRF Attack
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // =================================================
            // Gọi service xóa sản phẩm theo ID
            // =================================================

            _productService.Delete(id);

            // =================================================
            // Sau khi xóa xong
            // quay lại trang danh sách
            // =================================================

            return RedirectToAction(nameof(Index));
        }
    }
}