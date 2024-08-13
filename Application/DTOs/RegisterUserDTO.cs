using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace Application.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        public string? UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string? UserEmail { get; set; } = string.Empty;

        [Required]
        public string? Password { get; set; } = string.Empty;

        [Required, Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public AddressEnum? AddressType { get; set; }

        [Required]
        public string? Address { get; set; } = string.Empty;

        public string? AddressTitle { get; set; } = "Default Title";
        ////[Required]
        ////public string? UserName { get; set; }=string.Empty;
        ////[Required, EmailAddress]
        ////public string? UserEmail { get; set; } = string.Empty;
        ////[Required]
        ////public string? Password { get; set; } = string.Empty;
        ////[Required, Compare(nameof(Password))]
        ////public string? ConfirmPassword { get; set; } = string.Empty;
        ////[Required]
        ////public RoleEnum? RoleType { get; set; }
        //////[Required]
        //////public AddressEnum? Address { get; set; } = AddressEnum.Office;
    } 
}
