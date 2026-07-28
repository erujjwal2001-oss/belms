using BELMS.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = ValidationMessages.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationMessages.EmailInvalid)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
    public string Password { get; set; } = string.Empty;
}
