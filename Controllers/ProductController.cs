// File: Controllers/ProductController.cs

using Microsoft.AspNetCore.Mvc;

using CNPMFastFood.Data;
using CNPMFastFood.Models;

namespace CNPMFastFood.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext
            _context;

        private readonly IWebHostEnvironment
            _environment;

        public ProductController(
            AppDbContext context,

            IWebHostEnvironment environment)
        {
            _context = context;

            _environment = environment;
        }

        // =========================
        // CREATE PAGE
        // =========================

        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // CREATE PRODUCT
        // =========================

        [HttpPost]
        public IActionResult Create(Product product)
        {
            // kiểm tra upload file

            if (product.ImageFile != null)
            {
                // tên file

                string fileName =
                    Guid.NewGuid()
                    + Path.GetExtension(
                        product.ImageFile.FileName);

                // đường dẫn folder

                string folderPath =
                    Path.Combine(
                        _environment.WebRootPath,

                        "images/products");

                // tạo folder nếu chưa có

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(
                        folderPath);
                }

                // full path file

                string filePath =
                    Path.Combine(
                        folderPath,
                        fileName);

                // save file

                using (var stream =
                       new FileStream(
                           filePath,
                           FileMode.Create))
                {
                    product.ImageFile
                        .CopyTo(stream);
                }

                // lưu DB

                product.ImageUrl =
                    "/images/products/"
                    + fileName;
            }

            // save database

            _context.Products.Add(product);

            _context.SaveChanges();

            return RedirectToAction(
                "Index",
                "Home");
        }
    }
}