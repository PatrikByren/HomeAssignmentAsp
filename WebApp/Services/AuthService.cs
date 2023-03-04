using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Context;
using WebApp.Migrations.Identity;
using WebApp.Models.Entities;
using WebApp.Models.Forms;

namespace WebApp.Services
{
    public class AuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityContext _identityContext;
        private readonly ProfileService _profileService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IdentityContext identityContext, ProfileService profileService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityContext = identityContext;
            _profileService = profileService;
            _roleManager = roleManager;
        }

        public async Task<bool> RegisterAsync(SignUpForm form)
        {
            var identityUser = new IdentityUser
            {
                Email = form.Email,
                UserName = form.Email,
                PhoneNumber = form.PhoneNumber.ToString()
            };
            var result = await _userManager.CreateAsync(identityUser, form.Password);
            if (result.Succeeded)
            {
                var userProfileEntity = new UserProfileEntity
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
                };
                if (form.ProfileImage != null)
                {
                    userProfileEntity.ImageName = await _profileService.UploadProfileImageAsync(form.ProfileImage);
                }
                _identityContext.Add(userProfileEntity);
                await _identityContext.SaveChangesAsync();

                try { await _userManager.AddToRoleAsync(identityUser, "User"); }
                catch { }

                var signInResult = await _signInManager.PasswordSignInAsync(identityUser, form.Password, false, false);
                if (signInResult.Succeeded)
                    return true;
                else
                    return false;
            }
            else { return false; }
        }
        public async Task<bool> RegisterAdminAsync(SignUpForm form)
        {
            
            if (!await _roleManager.Roles.AnyAsync())
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
                    var userProfileEntity = new UserProfileEntity
                {
                    UserId = identityUser.Id,
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    StreetAdress = form.StreetAdress,
                    PostalCode = form.PostalCode,
                    City = form.City,
                    PhoneNumber = form.PhoneNumber,
                    Company = form.Company ?? null,
                    CreatedAt = form.CreatedAt,
                };
                if (form.ProfileImage != null)
                {
                userProfileEntity.ImageName = await _profileService.UploadProfileImageAsync(form.ProfileImage);
                }
                _identityContext.UserProfiles.Add(userProfileEntity);

                await _identityContext.SaveChangesAsync();

                await _userManager.AddToRoleAsync(identityUser, "Administrator");

                var signInResult = await _signInManager.PasswordSignInAsync(identityUser, form.Password, false, false);
                if (signInResult.Succeeded)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}
