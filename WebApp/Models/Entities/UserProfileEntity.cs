using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Entities
{
    public class UserProfileEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public string StreetAdress { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;
        public string? ImageName { get; set; }

        public string City { get; set; } = string.Empty;
        public int PhoneNumber { get; set; }
        public string? Company { get; set; }
        public virtual IdentityUser User { get; set; } = null!;
    }
}
