using CNPMFastFood.Models;
using CNPMFastFood.Services;
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: /Admin/Product
        public IActionResult Index()
        {
            var products = _productService.GetAll();
            return View(products);
        }

        // GET: /Admin/Product/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _productService.Add(product);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Product/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Admin/Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _productService.Update(product);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}