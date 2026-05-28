using CNPMFastFood.Data;
using CNPMFastFood.Models;
using Microsoft.EntityFrameworkCore;

namespace CNPMFastFood.Services
{
    // =========================================================
    // PRODUCT SERVICE
    // ---------------------------------------------------------
    // Service này xử lý toàn bộ nghiệp vụ liên quan đến sản phẩm:
    //
    // - Lấy danh sách sản phẩm
    // - Phân trang sản phẩm
    // - Tìm kiếm sản phẩm
    // - Thêm sản phẩm
    // - Cập nhật sản phẩm
    // - Xóa sản phẩm
    // - Upload ảnh sản phẩm
    // - Xóa ảnh sản phẩm
    //
    // ProductService làm việc với:
    // - Database thông qua AppDbContext
    // - Thư mục lưu ảnh thông qua IWebHostEnvironment
    // =========================================================
    public class ProductService
    {
        // =====================================================
        // DbContext kết nối database
        // =====================================================
        private readonly AppDbContext _context;

        // =====================================================
        // IWebHostEnvironment
        // -----------------------------------------------------
        // Dùng để truy cập thư mục wwwroot
        // phục vụ upload/xóa ảnh
        // =====================================================
        private readonly IWebHostEnvironment _environment;

        // =====================================================
        // CONSTRUCTOR
        // -----------------------------------------------------
        // Dependency Injection:
        // ASP.NET Core sẽ tự động inject:
        // - AppDbContext
        // - IWebHostEnvironment
        // =====================================================
        public ProductService(
            AppDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // =====================================================
        // LẤY TOÀN BỘ SẢN PHẨM
        // =====================================================
        public List<Product> GetAll()
        {
            return _context.Products

                // Include Reviews để lấy đánh giá sản phẩm
                .Include(p => p.Reviews)

                // Sản phẩm mới nhất hiển thị trước
                .OrderByDescending(p => p.CreatedAt)

                // Chuyển thành List
                .ToList();
        }

        // =====================================================
        // LẤY DANH SÁCH SẢN PHẨM CÓ PHÂN TRANG
        // =====================================================
        public ProductPageResult GetPagedProducts(
            int? categoryId,
            string? sortPrice,
            int page,
            int pageSize)
        {
            // -------------------------------------------------
            // Tạo query từ bảng Products
            // -------------------------------------------------
            var query = _context.Products
                .Include(p => p.Reviews)
                .AsQueryable();

            // =================================================
            // FILTER THEO CATEGORY
            // =================================================
            if (categoryId != null)
            {
                query = query.Where(p =>
                    p.CategoryId == categoryId);
            }

            // =================================================
            // SẮP XẾP GIÁ
            // =================================================

            // Giá tăng dần
            if (sortPrice == "asc")
            {
                query = query.OrderBy(p => p.Price);
            }

            // Giá giảm dần
            else if (sortPrice == "desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }

            // Mặc định: sản phẩm mới nhất
            else
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }

            // =================================================
            // TỔNG SỐ SẢN PHẨM
            // =================================================
            int totalProducts = query.Count();

            // =================================================
            // TÍNH TỔNG SỐ TRANG
            // -------------------------------------------------
            // Math.Ceiling làm tròn lên
            //
            // Ví dụ:
            // 25 sản phẩm / 10 mỗi trang = 2.5
            // => làm tròn lên thành 3 trang
            // =================================================
            int totalPages =
                (int)Math.Ceiling(
                    totalProducts / (double)pageSize
                );

            // =================================================
            // LẤY DỮ LIỆU THEO TRANG
            // -------------------------------------------------
            // Skip:
            // Bỏ qua các sản phẩm trước đó
            //
            // Take:
            // Lấy số lượng sản phẩm theo pageSize
            // =================================================
            var products = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // =================================================
            // TRẢ KẾT QUẢ PHÂN TRANG
            // =================================================
            return new ProductPageResult
            {
                Products = products,
                TotalPages = totalPages
            };
        }

        // =====================================================
        // LẤY SẢN PHẨM THEO ID
        // =====================================================
        public Product? GetById(int id)
        {
            return _context.Products

                // Lấy kèm review
                .Include(p => p.Reviews)

                // Tìm theo Id
                .FirstOrDefault(p => p.Id == id);
        }

        // =====================================================
        // TÌM KIẾM SẢN PHẨM
        // =====================================================
        public List<Product> SearchProducts(string keyword)
        {
            // -------------------------------------------------
            // Nếu keyword rỗng
            // -------------------------------------------------
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Product>();
            }

            return _context.Products

                // Lấy kèm reviews
                .Include(p => p.Reviews)

                // Tìm sản phẩm chứa keyword
                .Where(p => p.Name.Contains(keyword))

                // Giới hạn tối đa 8 sản phẩm
                .Take(8)

                .ToList();
        }

        // =====================================================
        // THÊM SẢN PHẨM
        // =====================================================
        public void Add(Product product)
        {
            // -------------------------------------------------
            // Nếu có upload ảnh
            // -------------------------------------------------
            if (product.ImageFile != null)
            {
                // Lưu ảnh và lấy đường dẫn
                product.ImageUrl =
                    SaveImage(product.ImageFile);
            }

            // Gán ngày tạo
            product.CreatedAt = DateTime.Now;

            // Thêm sản phẩm vào database
            _context.Products.Add(product);

            // Lưu database
            _context.SaveChanges();
        }

        // =====================================================
        // CẬP NHẬT SẢN PHẨM
        // =====================================================
        public void Update(Product product)
        {
            // Tìm sản phẩm hiện tại
            var existing =
                _context.Products.Find(product.Id);

            // Nếu không tồn tại
            if (existing == null)
            {
                return;
            }

            // -------------------------------------------------
            // Cập nhật thông tin sản phẩm
            // -------------------------------------------------
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Description = product.Description;
            existing.Slug = product.Slug;
            existing.CategoryId = product.CategoryId;
            existing.Featured = product.Featured;
            existing.Stock = product.Stock;

            // -------------------------------------------------
            // Nếu có ảnh mới
            // -------------------------------------------------
            if (product.ImageFile != null)
            {
                // Xóa ảnh cũ
                DeleteImage(existing.ImageUrl);

                // Lưu ảnh mới
                existing.ImageUrl =
                    SaveImage(product.ImageFile);
            }

            // Lưu database
            _context.SaveChanges();
        }

        // =====================================================
        // XÓA SẢN PHẨM
        // =====================================================
        public void Delete(int id)
        {
            // Tìm sản phẩm theo Id
            var product =
                _context.Products.Find(id);

            // Nếu không tồn tại
            if (product == null)
            {
                return;
            }

            // Xóa ảnh sản phẩm khỏi thư mục
            DeleteImage(product.ImageUrl);

            // Xóa sản phẩm khỏi database
            _context.Products.Remove(product);

            // Lưu thay đổi
            _context.SaveChanges();
        }

        // =====================================================
        // LƯU ẢNH
        // =====================================================
        private string SaveImage(IFormFile imageFile)
        {
            // -------------------------------------------------
            // Tạo tên file ngẫu nhiên bằng Guid
            // để tránh trùng tên ảnh
            // -------------------------------------------------
            string fileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(imageFile.FileName);

            // -------------------------------------------------
            // Đường dẫn thư mục:
            // wwwroot/images/products
            // -------------------------------------------------
            string folderPath =
                Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "products"
                );

            // -------------------------------------------------
            // Nếu thư mục chưa tồn tại thì tạo mới
            // -------------------------------------------------
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // -------------------------------------------------
            // Tạo đường dẫn đầy đủ của file ảnh
            // -------------------------------------------------
            string filePath =
                Path.Combine(folderPath, fileName);

            // -------------------------------------------------
            // Ghi file ảnh vào server
            // -------------------------------------------------
            using (var stream =
                   new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            // -------------------------------------------------
            // Trả về URL lưu trong database
            // -------------------------------------------------
            return "/images/products/" + fileName;
        }

        // =====================================================
        // XÓA ẢNH
        // =====================================================
        private void DeleteImage(string? imageUrl)
        {
            // Nếu imageUrl rỗng thì bỏ qua
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            // -------------------------------------------------
            // Chuyển URL thành đường dẫn vật lý
            // -------------------------------------------------
            string imagePath =
                Path.Combine(
                    _environment.WebRootPath,
                    imageUrl.TrimStart('/')
                );

            // -------------------------------------------------
            // Nếu file tồn tại thì xóa
            // -------------------------------------------------
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }

    // =========================================================
    // CLASS KẾT QUẢ PHÂN TRANG
    // ---------------------------------------------------------
    // Dùng để trả dữ liệu phân trang sản phẩm
    // =========================================================
    public class ProductPageResult
    {
        // Danh sách sản phẩm của trang hiện tại
        public List<Product> Products { get; set; } = new();

        // Tổng số trang
        public int TotalPages { get; set; }
    }
}