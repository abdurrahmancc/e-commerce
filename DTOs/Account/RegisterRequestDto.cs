using e_commerce.Enums;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTOs.Account
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Minimum 2 length of FirstName")]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*]).{6,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }
}
