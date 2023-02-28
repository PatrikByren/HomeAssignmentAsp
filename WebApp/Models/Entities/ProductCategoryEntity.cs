using Microsoft.EntityFrameworkCore;

namespace WebApp.Models.Entities
{
    [Index(nameof(Category), IsUnique = true)]

    public class ProductCategoryEntity
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
