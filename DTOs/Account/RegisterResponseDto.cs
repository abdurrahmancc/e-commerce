using e_commerce.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTOs.Account
{
    public class RegisterResponseDto
    {
        public Guid Id { get; set; }
        public string UserName {get; set;}
        public Status Status { get; set; }
    }
}
