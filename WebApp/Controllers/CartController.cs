using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
