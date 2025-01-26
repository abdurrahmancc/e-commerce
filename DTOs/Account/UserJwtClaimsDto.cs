using e_commerce.Enums;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTOs.Account
{
    public class UserJwtClaimsDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        public List<UserRole> Role { get; set; } = new List<UserRole> { UserRole.User };

        public string? Country { get; set; }

        public string? CountryCode { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
