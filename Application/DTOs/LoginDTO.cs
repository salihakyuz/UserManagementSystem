//using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class LoginDTO
    {
        [Required, EmailAddress]
        public string? UserEmail { get; set; } = string.Empty;
        [Required]
        public string? Password { get; set; } = string.Empty;
    }
}
