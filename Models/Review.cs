namespace CNPMFastFood.Models
{
    public class Review
    {
        public int Id { get; set; }

        // user review
        public string UserName { get; set; }

        // số sao
        public int Rating { get; set; }

        // nội dung đánh giá
        public string Comment { get; set; }

        // sản phẩm nào
        public int ProductId { get; set; }

        public Product Product { get; set; }

        // ngày review
        public DateTime CreatedAt { get; set; }
    }
}