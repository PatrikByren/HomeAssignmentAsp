using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Globalization;
using WebApp.Context;
using WebApp.Models.Entities;
using WebApp.Models.Forms;
using WebApp.Models.Identity;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AuthService _auth;
        private readonly UserSerivce _userSerivce;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AuthService auth, UserSerivce userSerivce)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _auth = auth;
            _userSerivce = userSerivce;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userInfo = @User.FindFirst("UserId").Value;
            if (userInfo != null)
            {
                var userModel = await _userSerivce.GetUserAccountAsync(userInfo);
                return View(userModel);
            }
            return View();
        }

        public async Task<IActionResult> Register(string ReturnUrl = null!)
        {
            if (!await _userManager.Users.AnyAsync())
                return RedirectToAction("Configure", "Admin");

            var form = new SignUpForm
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/")
            };
            return View(form);
        }
        [HttpPost]
        public async Task<IActionResult> Register(SignUpForm form)
        {
            if(form.TermsAndAggrements != true) 
            { ModelState.AddModelError(string.Empty, "You have to check Terms And Aggrements"); }
            if(ModelState.IsValid)
            {
                if(await _userManager.Users.AnyAsync(x => x.Email == form.Email))
                {
                    ModelState.AddModelError(string.Empty, "A user whit the same email already exists");
                    return View(form);
                }
                if (await _auth.RegisterAsync(form)) { return LocalRedirect(form.ReturnUrl); }
                else { return View(form); }
            }
            ModelState.AddModelError(string.Empty, "Unable to create an Account");
            return View(form);
        }
        public IActionResult Login(string ReturnUrl = null!)
        {
            var form = new SignInForm
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/")
            };
            return View(form);
        }
        [HttpPost]
        public async Task<IActionResult> Login(SignInForm form)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(form.Email, form.Password, form.KeepMeLoggedIn, false);
                if (signInResult.Succeeded)
                    return LocalRedirect(form.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Incorrect email or password");
            return View(form);
        }
        public async Task<IActionResult> LogOut()
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            return LocalRedirect("/");
        }
        /*public IActionResult UserHandler()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserHandler(string id)
        {
            if (ModelState.IsValid)
            {
                var userAccount = await _userSerivce.GetUserAccountAsync(id);
                return View(userAccount);
            }
            return View();
        }*/
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserProfileModel user)
        {
            if (ModelState.IsValid)
            {
                await _userSerivce.UpdateUserAsync(user);
                return LocalRedirect(user.ReturnUrl ?? Url.Content("~/"));
            }
            ModelState.AddModelError(string.Empty, "Changes not saved!");
            return LocalRedirect(user.ReturnUrl ?? Url.Content("~/"));
        }
        [Authorize]
        public async Task<IActionResult> DeleteUser(UserProfileModel user)
        {
            var id = user.Id;
            if (id != null!)
            {
            await _userSerivce.DeleteUserAsync(id);
            return LocalRedirect(user.ReturnUrl ?? Url.Content("~/"));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Changes not saved!");
            }
            return LocalRedirect(user.ReturnUrl ?? Url.Content("~/"));
        }

    }
}
