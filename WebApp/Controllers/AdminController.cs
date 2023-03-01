using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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



        public AdminController(ProductService productService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IdentityContext identityContext, RoleManager<IdentityRole> roleManager, UserSerivce userService)
        {
            _productService = productService;
            _userManager = userManager;
            _signInManager = signInManager;
            _identityContext = identityContext;
            _roleManager = roleManager;
            _userService = userService;
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
                if(!await _roleManager.Roles.AnyAsync())
                {
                    try { await _roleManager.CreateAsync(new IdentityRole("Administrator")); } catch { }
                    try { await _roleManager.CreateAsync(new IdentityRole("User Manager")); } catch { }
                    try { await _roleManager.CreateAsync(new IdentityRole("Product Manager")); } catch { }
                    try { await _roleManager.CreateAsync(new IdentityRole("User")); } catch { }
                }


                var identityUser = new IdentityUser
                {
                    Email = form.Email,
                    UserName = form.Email
                };
                var result = await _userManager.CreateAsync(identityUser, form.Password);
                if (result.Succeeded)
                {
                    _identityContext.UserProfiles.Add(new UserProfileEntity
                    {
                        UserId = identityUser.Id,
                        FirstName = form.FirstName,
                        LastName = form.LastName,
                        StreetAdress = form.StreetAdress,
                        PostalCode = form.PostalCode,
                        City = form.City,
                        PhoneNumber = form.PhoneNumber,
                        Company = form.Company ?? null,
                        CreatedAt = form.CreatedAt
                    });
                    await _identityContext.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(identityUser, "Administrator");

                    var signInResult = await _signInManager.PasswordSignInAsync(identityUser, form.Password, false, false);
                    if (signInResult.Succeeded)
                        return LocalRedirect(form.ReturnUrl);
                    else
                        return RedirectToAction("SignIn", "Account");
                }
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
                
                //ModelState.AddModelError(string.Empty, "Created an Product");
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
