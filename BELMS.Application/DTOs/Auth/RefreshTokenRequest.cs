using BELMS.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.Auth;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = ValidationMessages.RefreshTokenRequired)]
    public string RefreshToken { get; set; } = string.Empty;
}
