// File: Services/ProductService.cs
// Mô tả: CRUD Product sử dụng SQLite (Entity Framework)

using CNPMFastFood.Data;
using CNPMFastFood.Models;
using System.Collections.Generic;
using System.Linq;

namespace CNPMFastFood.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        // Constructor: inject DbContext
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // LẤY DANH SÁCH SẢN PHẨM
        // =========================
        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        // =========================
        // LẤY THEO ID
        // =========================
        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }

        // =========================
        // THÊM SẢN PHẨM
        // =========================
        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        // =========================
        // CẬP NHẬT SẢN PHẨM
        // =========================
        public void Update(Product product)
        {
            var existing = GetById(product.Id);

            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;

                _context.SaveChanges();
            }
        }

        // =========================
        // XÓA SẢN PHẨM
        // =========================
        public void Delete(int id)
        {
            var product = GetById(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}