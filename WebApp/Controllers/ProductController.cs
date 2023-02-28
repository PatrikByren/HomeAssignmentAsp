using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Products;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string skuForm)
        {
            if (skuForm == null!) skuForm = "11";
            try
            {
                var productsList = await _productService.GetProductsAsync();
                var products = new ProductDescriptionModel { Cards= productsList, Sku=skuForm };
                return View(products);
            }
            catch { return View(); }
        }
        public IActionResult Sale()
        {
            return View();
        }
    }
}