namespace WebApp.Services
{
    public class ProfileService
    {
        private readonly IWebHostEnvironment _environment;

        public ProfileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadProfileImageAsync(IFormFile profilImage)
        {
            var wwwroot = $"{_environment.WebRootPath}/Images/Profiles";
            var imageName = $"profile_{Guid.NewGuid()}{Path.GetExtension(profilImage.FileName)}";
            string filePath = $"{wwwroot}/{imageName}";

            using var fs = new FileStream(filePath, FileMode.Create);
            await profilImage.CopyToAsync(fs);
            return imageName;
        }
    }
}
