using BELMS.Application.DTOs;
using BELMS.Application.DTOs.Auth;
using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IAuthService
{
    Task<Result<TokenResponseDto>> LoginAsync(LoginRequest request);
    Task<Result<TokenResponseDto>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<Result> LogoutAsync(string refreshToken);
}
