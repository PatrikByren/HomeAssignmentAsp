using Microsoft.EntityFrameworkCore;
using WebApp.Models.Entities;

namespace WebApp.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<BrandEntity> Brands { get; set; }
        public DbSet<ColorEntity> Colors { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<PriceEntity> Prices { get; set; }
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
        public DbSet<ProductDescriptionEntity> ProductDescriptions { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductReviewEntity> ProductReviews { get; set; }
        public DbSet<SubscribesEntity> Subscribes { get; set; }
        public DbSet<ReleseYearEntity> ReleseYears { get; set; }

    }
}

