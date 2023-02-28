using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using WebApp.Models.Entities;

namespace WebApp.Models.Products
{
    public class CreateProductModel
    {
        [Required]
        [Display(Name = "SKU Number")]
        public string SKU { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [Display(Name = "Discount Price(Optional)")]
        public decimal? DiscountPrice { get; set; }
        [Required]
        [Display(Name = "Brand")]
        public string Brand { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Color")]
        public string Color { get; set; } = string.Empty;
        [Required]
        [Display(Name = "ReleseYear")]
        public int ReleseYear { get; set; }
        [Required]
        [Display(Name = "Product active")]
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
