namespace WebApp.Models.Identity
{
    public class UserProfileModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public int PhoneNumber { get; set; }
        public string StreetName { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Company { get; set; }
        public string? ReturnUrl { get; set; }
        public string? Role { get; set;}
        public string? ProfileImage { get; set; }
        public IFormFile? NewProfileImage { get; set; }
    }
}
