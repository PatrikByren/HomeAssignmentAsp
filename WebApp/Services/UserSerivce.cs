using Microsoft.AspNetCore.Identity;
using WebApp.Context;
using WebApp.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Services
{
    public class UserSerivce
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityContext _identityContext;
        private readonly ProfileService _profileService;

        public UserSerivce(UserManager<IdentityUser> userManager, IdentityContext identityContext, ProfileService profileService)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _profileService = profileService;
        }
        [HttpGet]
        public async Task<UserAccountModel> GetUserAccountAsync(string id)
        {
            var identityProfile = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (identityProfile != null)
            {
                var userProfileModel = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == identityProfile.Id);
                if (userProfileModel != null)
                {
                    return new UserAccountModel
                    {
                        Id = userProfileModel.Id.ToString(),
                        UserId = identityProfile.Id,
                        FirstName = userProfileModel.FirstName,
                        LastName = userProfileModel.LastName,
                        Email = identityProfile.Email!,
                        PhoneNumber = userProfileModel.PhoneNumber.ToString(),
                        StreetName = userProfileModel.StreetAdress,
                        City = userProfileModel.City,
                        PostalCode = userProfileModel.PostalCode,
                        Company = userProfileModel.Company,
                        ImageName = userProfileModel.ImageName
                    };
                }
            }
            return null!;


        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UserProfileModel userProfileModel)
        {
            {
                try
                {
                    var userProfileEntity = await _identityContext.UserProfiles.FindAsync(userProfileModel.Id);
                    //if (userProfileEntity == null) { await _identityContext.UserProfiles.FindAsync(userProfileModel.Us); }
                    if (userProfileEntity != null)
                    {
                        userProfileEntity.FirstName = userProfileModel.FirstName;
                        userProfileEntity.LastName = userProfileModel.LastName;
                        userProfileEntity.PhoneNumber = userProfileModel.PhoneNumber;
                        userProfileEntity.City = userProfileModel.City;
                        userProfileEntity.PostalCode = userProfileModel.PostalCode;
                        userProfileEntity.Company = userProfileModel.Company;
                        userProfileEntity.StreetAdress = userProfileModel.StreetName;
                        userProfileEntity.PostalCode = userProfileModel.PostalCode.ToString();
                        if (userProfileModel.NewProfileImage != null)
                        {
                            userProfileEntity.ImageName = await _profileService.UploadProfileImageAsync(userProfileModel.NewProfileImage!);
                        }

                        _identityContext.Entry(userProfileEntity).State = EntityState.Modified;
                        await _identityContext.SaveChangesAsync();
                        if (userProfileModel.Role != null!)
                        try {
                            var userProfile = await _identityContext.UserProfiles.FindAsync(userProfileModel.Id);
                            var identityUser = await _identityContext.Users.FindAsync(userProfile!.UserId);
                                await _userManager.RemoveFromRolesAsync(identityUser!, new List<string>() { "User", "Product Manager", "User Manager", "Administrator" });
                                await _userManager.AddToRoleAsync(identityUser!, userProfileModel.Role);
                            }
                        catch { }

                        return new OkResult();
                    }
                    return new NotFoundResult();
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
                return new BadRequestResult();
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            {
                try
                {
                    var userProfileEntity = await _identityContext.UserProfiles.FindAsync(id);
                    if (userProfileEntity == null)
                        return new NotFoundResult();
                    _identityContext.UserProfiles.Remove(userProfileEntity);
                    var userEntity = await _identityContext.Users.FindAsync(userProfileEntity.UserId);
                    if (userEntity == null)
                        return new NotFoundResult();
                    _identityContext.Users.Remove(userEntity);
                    await _identityContext.SaveChangesAsync();
                    return new OkResult();
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
                return new BadRequestResult();
            }
        }
        [HttpGet]
        public async Task<IEnumerable<UserProfileModel>> GetAllUsersAsync()
        {
            var customerModel = new List<UserProfileModel>();

            try
            {
                foreach (var item in await _identityContext.UserProfiles.ToListAsync())
                {
                    var userEntity = await _identityContext.Users.FindAsync(item.UserId);
                    customerModel.Add(new UserProfileModel
                    {
                        Id = item.Id,
                        Email = userEntity!.UserName!,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PhoneNumber = item.PhoneNumber,
                        StreetName = item.StreetAdress, 
                        City = item.City,
                        PostalCode = item.PostalCode,
                        Company = item.Company
                    });
                }

                return customerModel;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return customerModel;
        }

    }
}
