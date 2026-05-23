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

        // GET PAGED PRODUCTS
        public ProductPageResult GetPagedProducts(
            int? categoryId,
            string? sortPrice,
            int page,
            int pageSize)
        {
            var query = _context.Products
                .Include(p => p.Reviews)
                .AsQueryable();

            // FILTER CATEGORY
            if (categoryId != null)
            {
                query = query.Where(p =>
                    p.CategoryId == categoryId);
            }

            // SORT PRICE
            if (sortPrice == "asc")
            {
                query = query.OrderBy(p => p.Price);
            }
            else if (sortPrice == "desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }

            int totalProducts = query.Count();

            int totalPages =
                (int)Math.Ceiling(
                    totalProducts / (double)pageSize
                );

            var products = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ProductPageResult
            {
                Products = products,
                TotalPages = totalPages
            };
        }

        // GET PRODUCT BY ID
        public Product? GetById(int id)
        {
            return _context.Products
                .Include(p => p.Reviews)
                .FirstOrDefault(p => p.Id == id);
        }

        // SEARCH PRODUCT
        public List<Product> SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Product>();
            }

            return _context.Products
                .Include(p => p.Reviews)
                .Where(p => p.Name.Contains(keyword))
                .Take(8)
                .ToList();
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
                    "products"
                );

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
                    imageUrl.TrimStart('/')
                );

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }

    // PAGINATION RESULT
    public class ProductPageResult
    {
        public List<Product> Products { get; set; } = new();

        public int TotalPages { get; set; }
    }
}