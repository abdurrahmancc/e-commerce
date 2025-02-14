using e_commerce.Enums;

namespace e_commerce.DTOs.Account
{
    public class LoginResponseDto
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public Status? Status { get; set; }
        public string Token { get; set; }
    }
}
