namespace WebApp.Models.Products
{
    public class ProductDescriptionModel
    {
        public string Sku { get; set; } = null!;
        public ICollection<ProductCardModel> Cards { get; set; }

    }
}
