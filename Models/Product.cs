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

        // mô tả
        public string? Description { get; set; }

        // slug url
        public string? Slug { get; set; }

        // category
        public int? CategoryId { get; set; }

        // sản phẩm nổi bật
        public bool Featured { get; set; }

        // tồn kho
        public int Stock { get; set; }

        // ngày tạo
        public DateTime CreatedAt { get; set; }

        // upload file
        // không tạo cột trong DB

        [NotMapped]
        public IFormFile? ImageFile
        {
            get; set;
        }

        // reviews

        public ICollection<Review> Reviews
        {
            get; set;
        } = new List<Review>();
    }
}