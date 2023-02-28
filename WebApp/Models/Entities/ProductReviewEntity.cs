namespace WebApp.Models.Entities
{
    public class ProductReviewEntity
    {
        public int Id { get; set; }
        public string ProductSKU { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Today;

        public ProductEntity Product { get; set; } = null!;
    }
}
