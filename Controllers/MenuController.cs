using Microsoft.AspNetCore.Mvc; // Thư viện hỗ trợ Controller, IActionResult, Json,...
using CNPMFastFood.Services; // Chứa ProductService xử lý nghiệp vụ sản phẩm

namespace CNPMFastFood.Controllers
{
    // Controller xử lý trang menu sản phẩm
    public class MenuController : Controller
    {
        // Service dùng để lấy, lọc, tìm kiếm sản phẩm
        private readonly ProductService _productService;

        // Constructor Dependency Injection
        public MenuController(ProductService productService)
        {
            // Gán service được inject vào biến dùng trong controller
            _productService = productService;
        }

        // =========================
        // MENU PAGE
        // Hiển thị danh sách sản phẩm
        // Có lọc danh mục, sắp xếp giá, phân trang
        // =========================

        public IActionResult Index(
            int? categoryId,     // Id danh mục, có thể null nếu không lọc
            string? sortPrice,   // Kiểu sắp xếp giá, ví dụ: tăng dần / giảm dần
            int page = 1)        // Trang hiện tại, mặc định là trang 1
        {
            // Số sản phẩm hiển thị trên mỗi trang
            int pageSize = 8;

            // Lấy danh sách sản phẩm đã được lọc, sắp xếp và phân trang
            var result =
                _productService.GetPagedProducts(
                    categoryId,  // Lọc theo danh mục
                    sortPrice,   // Sắp xếp theo giá
                    page,        // Trang hiện tại
                    pageSize     // Số sản phẩm mỗi trang
                );

            // Lưu categoryId vào ViewBag để View biết danh mục đang chọn
            ViewBag.CategoryId = categoryId;

            // Lưu kiểu sắp xếp giá để giữ trạng thái dropdown/sort
            ViewBag.SortPrice = sortPrice;

            // Lưu trang hiện tại
            ViewBag.CurrentPage = page;

            // Lưu tổng số trang để hiển thị phân trang
            ViewBag.TotalPages = result.TotalPages;

            // Trả danh sách sản phẩm sang View Index.cshtml
            return View(result.Products);
        }

        // =========================
        // SEARCH PRODUCT - AJAX
        // Tìm kiếm sản phẩm theo từ khóa
        // Trả kết quả dạng JSON
        // =========================

        [HttpGet]
        public IActionResult Search(string keyword)
        {
            // Gọi service tìm sản phẩm theo keyword
            var products = _productService.SearchProducts(keyword);

            // Trả dữ liệu JSON cho client, thường dùng cho AJAX/search realtime
            return Json(products.Select(p => new
            {
                // Id sản phẩm
                id = p.Id,

                // Tên sản phẩm
                name = p.Name,

                // Giá sản phẩm
                price = p.Price,

                // Đường dẫn hình ảnh
                image = p.ImageUrl,

                // Nếu chưa có mô tả thì dùng mô tả mặc định
                description = string.IsNullOrEmpty(p.Description)
                    ? "Chưa có mô tả sản phẩm"
                    : p.Description,

                // Tính điểm đánh giá trung bình
                avgRating = p.Reviews != null && p.Reviews.Any()
                    ? p.Reviews.Average(r => r.Rating)
                    : 0,

                // Đếm số lượng đánh giá
                reviewCount = p.Reviews != null
                    ? p.Reviews.Count
                    : 0,

                // Lấy 2 đánh giá mới nhất
                reviews = p.Reviews != null
                    ? p.Reviews
                        .OrderByDescending(r => r.CreatedAt)
                        .Take(2)
                        .Select(r => new
                        {
                            // Tên người đánh giá
                            userName = r.UserName,

                            // Số sao đánh giá
                            rating = r.Rating,

                            // Nội dung bình luận
                            comment = r.Comment,

                            // Ngày đánh giá, định dạng dd/MM/yyyy
                            createdAt = r.CreatedAt.ToString("dd/MM/yyyy")
                        })
                        .ToList()
                    : null
            }));
        }
    }
}