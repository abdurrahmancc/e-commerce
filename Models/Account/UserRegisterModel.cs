using e_commerce.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.Models.Account
{
    public class UserRegisterModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Minimum 2 length of FirstName")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*]).{6,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string PhotoUrl { get; set; }

        public List<UserRole> Roles { get; set; } = new List<UserRole> { UserRole.User};

        public string IPAddress { get; set; }

        public Status Status { get; set; } = Status.Inactive;

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public List<string> LoginDevices { get; set; } = new List<string>();

        public DateTime LastLoginDate { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
    }
}
