using e_commerce.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace e_commerce.Core
{
    public class UserTokenContext
    {
        public Guid? Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public List<UserRole> Roles { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
