using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Entities
{
    public class ProductEntity
    {
        [Key]
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int PriceId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int DescriptionId { get; set; }
        public int ColorId { get; set; }
        public int? ReviewsId { get; set; }
        public int ReleseYearId { get; set; }
        public int? ImageId { get; set; }
        public string? ImageName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = new DateTime();
        public ICollection<ProductReviewEntity>? Reviews { get; set; } = new List<ProductReviewEntity>();
        public PriceEntity Price { get; set; }
        public ImageEntity? Images { get; set; }
        public BrandEntity Brand { get; set; }
        public ProductCategoryEntity Category { get; set; }
        public ProductDescriptionEntity Description { get; set; }
        public ReleseYearEntity ReleseYear { get; set; }
        public ColorEntity Color { get; set; }

    }
}

