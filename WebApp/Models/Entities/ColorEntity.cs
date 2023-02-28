using Microsoft.EntityFrameworkCore;

namespace WebApp.Models.Entities
{
    [Index(nameof(Name), IsUnique = true)]

    public class ColorEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
    }
}
