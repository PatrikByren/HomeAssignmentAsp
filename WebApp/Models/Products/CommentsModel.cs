namespace WebApp.Models.Products
{
    public class CommentsModel
    {
        public string ProductSku { get; set; } = null!;
        public int? Ratings { get; set; }
        public string? Comments { get; set; } = string.Empty;
        public string? PostBy { get; set; }
    }
}
