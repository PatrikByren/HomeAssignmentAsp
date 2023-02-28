using Microsoft.AspNetCore.Identity;
using WebApp.Context;
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

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IdentityContext identityContext, ProfileService profileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityContext = identityContext;
            _profileService = profileService;
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
    }
}
