using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using WebApp.Context;
using WebApp.Models.Entities;
using WebApp.Models.Forms;
using WebApp.Models.Identity;
using WebApp.Models.Products;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {
        private readonly ProductService _productService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityContext _identityContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserSerivce _userService;
        private readonly AuthService _authService;



        public AdminController(ProductService productService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IdentityContext identityContext, RoleManager<IdentityRole> roleManager, UserSerivce userService, AuthService authService)
        {
            _productService = productService;
            _userManager = userManager;
            _signInManager = signInManager;
            _identityContext = identityContext;
            _roleManager = roleManager;
            _userService = userService;
            _authService = authService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Configure(string ReturnUrl = null!)
        {

            if (await _userManager.Users.AnyAsync())
                return RedirectToAction("SignIn", "Account");

            var form = new SignUpForm
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/")
            };
            return View(form);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Configure(SignUpForm form)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _authService.RegisterAdminAsync(form);
                if (signInResult == true)
                    return LocalRedirect(form.ReturnUrl);
                else
                    return RedirectToAction("SignIn", "Account");
            }
            ModelState.AddModelError(string.Empty, "Unable to create an Account");
            return View(form);
        }

        [Authorize(Roles = "Product Manager, Administrator")]
        public IActionResult CreateProduct()
        {
            return View();
        }
        [Authorize(Roles = "Product Manager, Administrator")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductModel product)
        {
            if (ModelState.IsValid)
            {
                if (product.DiscountPrice == null) product.DiscountPrice = 0;
                await _productService.CreateProductAsync(product);
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Product Created!");
                return View();

            }
            ModelState.AddModelError(string.Empty, "Unable to create an Product");
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Index(string ReturnUrl = null!)
        {
            /*var viewModel = new AdminIndexViewModel();
            viewModel.Users = await _userService.GetAllUsersAsync();
            viewModel.ReturnUrl = ReturnUrl ?? Url.Context("/");*/

            var userProfileModel = await _userService.GetAllUsersAsync();
            
            return View(userProfileModel);
        }
    }
}
