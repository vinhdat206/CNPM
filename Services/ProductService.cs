// ================================
// File: Services/ProductService.cs
// ================================

using CNPMFastFood.Data;
using CNPMFastFood.Models;

using Microsoft.EntityFrameworkCore;

namespace CNPMFastFood.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductService(
            AppDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET ALL PRODUCTS
        public List<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.Reviews)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }

        // GET PRODUCT BY ID
        public Product? GetById(int id)
        {
            return _context.Products
                .Include(p => p.Reviews)
                .FirstOrDefault(p => p.Id == id);
        }

        // ADD PRODUCT
        public void Add(Product product)
        {
            if (product.ImageFile != null)
            {
                product.ImageUrl =
                    SaveImage(product.ImageFile);
            }

            product.CreatedAt = DateTime.Now;

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        // UPDATE PRODUCT
        public void Update(Product product)
        {
            var existing =
                _context.Products.Find(product.Id);

            if (existing == null)
            {
                return;
            }

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Description = product.Description;

            existing.Slug = product.Slug;
            existing.CategoryId = product.CategoryId;
            existing.Featured = product.Featured;
            existing.Stock = product.Stock;

            if (product.ImageFile != null)
            {
                DeleteImage(existing.ImageUrl);

                existing.ImageUrl =
                    SaveImage(product.ImageFile);
            }

            _context.SaveChanges();
        }

        // DELETE PRODUCT
        public void Delete(int id)
        {
            var product =
                _context.Products.Find(id);

            if (product == null)
            {
                return;
            }

            DeleteImage(product.ImageUrl);

            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        // SAVE IMAGE
        private string SaveImage(IFormFile imageFile)
        {
            string fileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(imageFile.FileName);

            string folderPath =
                Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "products");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath =
                Path.Combine(folderPath, fileName);

            using (var stream =
                   new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return "/images/products/" + fileName;
        }

        // DELETE IMAGE
        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            string imagePath =
                Path.Combine(
                    _environment.WebRootPath,
                    imageUrl.TrimStart('/'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}