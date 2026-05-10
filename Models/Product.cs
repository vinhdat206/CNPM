// File: Models/Product.cs

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Http;

namespace CNPMFastFood.Models
{
    public class Product
    {
        public int Id { get; set; }

        // tên món
        public string Name { get; set; }

        // giá
        public decimal Price { get; set; }

        // đường dẫn ảnh
        public string? ImageUrl { get; set; }

        // upload file
        // không tạo cột trong DB

        [NotMapped]
        public IFormFile? ImageFile
        {
            get; set;
        }
    }
}