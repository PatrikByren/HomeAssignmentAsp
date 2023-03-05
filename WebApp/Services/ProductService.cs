using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics;
using System.IO.Pipelines;
using WebApp.Context;
using WebApp.Models.Entities;
using WebApp.Models.Products;

namespace WebApp.Services
{
    public class ProductService
    {
        private readonly DataContext _context;
        private readonly IdentityContext _identityContext;


        public ProductService(DataContext context, IdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }
        public async Task<ProductCardModel> GetOneProductAsync(string sku)
        {
            try
            {
                var productModel = await _context.Products.FindAsync(sku);
                if (productModel == null)
                    return null!;
                var price = await _context.Prices.FindAsync(productModel.PriceId);


                if (productModel.CreatedAt.AddDays(10) < DateTime.Now)
                {
                    return new ProductCardModel
                    {
                        SKU = sku,
                        Name = productModel.Name,
                        Price = Math.Round(price!.Price, 2),
                        DiscountPrice = Math.Round(price.Price, 2),
                        TotalComments = productModel.Reviews.Count,
                        CreatedAt = productModel.CreatedAt,
                        New = true
                    };
                }
                else
                {
                    return new ProductCardModel
                    {
                        SKU = sku,
                        Name = productModel.Name,
                        Price = Math.Round(price!.Price, 2),
                        DiscountPrice = Math.Round(price.Price, 2),
                        TotalComments = productModel.Reviews.Count,
                        CreatedAt = productModel.CreatedAt,
                        New = false
                    };
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }

        public async Task<List<ProductCardModel>> GetProductsAsync()
        {
            try
            {
                var productCardModel = new List<ProductCardModel>();
                foreach (var item in await _context.Products.ToListAsync())
                {
                    var reviewList = new List<CommentsModel>();
                    int RatingNr = 0;
                    var reviews = await _context.ProductReviews.Where(x => x.ProductSKU == item.SKU).ToListAsync();
                    foreach (var reviewItem in reviews)
                    {
                        var name = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == reviewItem.UserId);
                        foreach (var review in reviews)
                        {
                            RatingNr += review.Rating;
                            reviewList.Add(new CommentsModel
                            {
                                Ratings = review.Rating,
                                Comments = review.Comment ?? "",
                                PostBy = name?.FirstName + " " ?? null! + name?.LastName ?? null!,
                            });
                        }
                            RatingNr = RatingNr / reviewList.Count;
                    }
                    var price = await _context.Prices.FindAsync(item.PriceId);
                    var description = await _context.ProductDescriptions.FindAsync(item.DescriptionId);
                    var category = await _context.ProductCategories.FindAsync(item.CategoryId);
                    //var reviews = await _context.ProductReviews.Where(x => x.ProductSKU == item.SKU).ToListAsync();
                    if (item.CreatedAt.AddDays(10) < DateTime.Today) 
                    { }
                        else 
                    { }
                    if (item.CreatedAt.AddDays(10) > DateTime.Today)
                    {
                        productCardModel.Add(new ProductCardModel
                        {
                            SKU = item.SKU,
                            Name = item.Name,
                            ImgUrl = item.ImageName ?? null!,
                            Price = Math.Round(price!.Price, 2),
                            DiscountPrice = Math.Round((decimal)price.DiscountPrice, 2),
                            TotalComments = item.Reviews.Count,
                            Rating = RatingNr,
                            Reviews = reviewList,
                            ShortDescription = description?.ShortText,
                            LongDescription = description?.LongText,
                            Category = category!.Category,
                            CreatedAt = item.CreatedAt,
                            New = true
                        }); 
                    }
                    else
                    {
                        productCardModel.Add(new ProductCardModel
                        {

                            SKU = item.SKU,
                            Name = item.Name,
                            ImgUrl = item.ImageName ?? null!,
                            Price = Math.Round(price!.Price, 2),
                            DiscountPrice = Math.Round((decimal)price.DiscountPrice, 2),
                            TotalComments = item.Reviews.Count,
                            Rating = RatingNr,
                            Reviews = reviewList,
                            ShortDescription = description?.ShortText,
                            LongDescription = description?.LongText,
                            Category = category!.Category,
                            CreatedAt = item.CreatedAt,
                        });
                     }
                }
                if (productCardModel != null!) { return productCardModel; }
                else return new List<ProductCardModel>();
            }
            catch { }
            return new List<ProductCardModel>();
        }
        public async Task<IActionResult> CreateProductAsync(CreateProductModel req)
        {

            var brandEntity =  await _context.Brands.FirstOrDefaultAsync(x => x.Name == req.Brand);
            if (brandEntity == null)
            {
                brandEntity = new BrandEntity() { Name = req.Brand! };
                _context.Brands.Add(brandEntity);
                await _context.SaveChangesAsync();
            }

            var priceEntity = await _context.Prices.FirstOrDefaultAsync(x => x.Price == req.Price && x.DiscountPrice == req.DiscountPrice);
            if (priceEntity == null)
            {
                priceEntity = new PriceEntity() { Price = req.Price!, DiscountPrice = (decimal)req.DiscountPrice};
                _context.Prices.Add(priceEntity);
                await _context.SaveChangesAsync();
            }
                
            var categoryEntity = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Category == req.Category);
            if (categoryEntity == null)
            {
                categoryEntity = new ProductCategoryEntity() { Category = req.Category! };
                _context.ProductCategories.Add(categoryEntity); await _context.SaveChangesAsync();
            }

            var descriptionEnity = await _context.ProductDescriptions.FirstOrDefaultAsync(x => x.ShortText == req.ShortDescription && x.LongText== req.LongDescription);
            if (descriptionEnity == null)
            {
                descriptionEnity = new ProductDescriptionEntity() { ShortText = req.ShortDescription!, LongText = req.LongDescription! };
                _context.ProductDescriptions.Add(descriptionEnity);
                await _context.SaveChangesAsync();
            }

            var colorEntity = await _context.Colors.FirstOrDefaultAsync(x => x.Name == req.Color);
            if (colorEntity == null)
            {
                colorEntity = new ColorEntity() { Name = req.Color! };
                _context.Colors.Add(colorEntity);
                await _context.SaveChangesAsync();
            }

            var releseYearEntity = await _context.ReleseYears.FirstOrDefaultAsync(x => x.Year == req.ReleseYear);
            if (releseYearEntity == null)
            {
                releseYearEntity = new ReleseYearEntity() { Year = req.ReleseYear! };
                _context.ReleseYears.Add(releseYearEntity);
                await _context.SaveChangesAsync();
            }
           

            try
            {

                var productEntity = new ProductEntity()
                {
                    SKU = req.SKU,
                    Name = req.Name,
                    BrandId = brandEntity.Id,
                    Brand= brandEntity,
                    PriceId=priceEntity.Id,
                    Price = priceEntity,
                    CategoryId= categoryEntity.Id,
                    Category= categoryEntity,
                    DescriptionId= categoryEntity.Id,
                    Description= descriptionEnity,
                    ColorId= colorEntity.Id,
                    Color = colorEntity,
                    ReleseYearId= releseYearEntity.Id,
                    ReleseYear= releseYearEntity,
                    CreatedAt = req.CreatedAt,
                    ImageName = req.ImageName
                };
                _context.Products.Add(productEntity);
                await _context.SaveChangesAsync();

                return new OkResult();
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }
    }

}
