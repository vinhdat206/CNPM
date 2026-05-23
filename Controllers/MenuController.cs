using Microsoft.AspNetCore.Mvc;
using CNPMFastFood.Services;

namespace CNPMFastFood.Controllers
{
    public class MenuController : Controller
    {
        private readonly ProductService _productService;

        public MenuController(
            ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index(
            int? categoryId,
            string? sortPrice,
            int page = 1)
        {
            int pageSize = 8;

            var result =
                _productService.GetPagedProducts(
                    categoryId,
                    sortPrice,
                    page,
                    pageSize
                );

            ViewBag.CategoryId = categoryId;
            ViewBag.SortPrice = sortPrice;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = result.TotalPages;

            return View(result.Products);
        }
        [HttpGet]
        public IActionResult Search(string keyword)
        {
            var products = _productService.SearchProducts(keyword);

            return Json(products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                price = p.Price,
                image = p.ImageUrl,
                description = string.IsNullOrEmpty(p.Description)
                    ? "Chưa có mô tả sản phẩm"
                    : p.Description,

                avgRating = p.Reviews != null && p.Reviews.Any()
                    ? p.Reviews.Average(r => r.Rating)
                    : 0,

                reviewCount = p.Reviews != null
                    ? p.Reviews.Count
                    : 0,

                reviews = p.Reviews != null
                    ? p.Reviews
                        .OrderByDescending(r => r.CreatedAt)
                        .Take(2)
                        .Select(r => new
                        {
                            userName = r.UserName,
                            rating = r.Rating,
                            comment = r.Comment,
                            createdAt = r.CreatedAt.ToString("dd/MM/yyyy")
                        })
                        .ToList()
                    : null
            }));
        }
    }
}