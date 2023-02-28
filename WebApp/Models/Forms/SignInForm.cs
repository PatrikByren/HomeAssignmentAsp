using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Forms
{
    public class SignInForm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Your Email")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Your Password")]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Keep me loggd in")]
        public bool KeepMeLoggedIn { get; set; } = false;
        public string ReturnUrl { get; set; } = null!;
    }
}
