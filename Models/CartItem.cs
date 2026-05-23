using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPMFastFood.Models
{
    public class CartItem
    {
        // khóa chính auto tăng
        [Key]
        public int CartItemId { get; set; }

        // id sản phẩm
        public int ProductId { get; set; }

        // user id
        public int? UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; } = 1;

        public string? ImageUrl { get; set; }

        [NotMapped]
        public decimal TotalPrice =>
            Price * Quantity;
    }
}