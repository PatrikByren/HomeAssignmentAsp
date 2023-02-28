using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Products;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService;

        public HomeController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productService.GetProductsAsync();
                return View(products);
            }
            catch { return View(); }
        }
    }
}
