using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Forms
{
    public class SignUpForm
    {
        [Required]
        [Display(Name ="Your First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Your Last Name")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [Display(Name = "Your Email")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Your Password")]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Your Password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Your Street Adress")]
        public string StreetAdress { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Your Postal Code")]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Your City")]
        public string City { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Your Phone Number")]
        public int PhoneNumber { get; set; }
        [Display(Name = "Your Company")]
        public string? Company { get; set; }
        [Display(Name = "Your Image")]
        public string? ImgUrl { get; set; }
        [Display(Name = "I have read and accepts the terms and agreements")]
        public IFormFile? ProfileImage { get; set; }
        public bool TermsAndAggrements { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ReturnUrl { get; set; } = null!;
    }
}
