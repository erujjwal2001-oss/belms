using BELMS.Application.DTOs;
using BELMS.Application.DTOs.Auth;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Entities;

namespace BELMS.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator) : IAuthService
{
    private const int RefreshTokenExpiryDays = 7;

    public async Task<Result<TokenResponseDto>> LoginAsync(LoginRequest request)
    {
        // Look up user by trimmed email
        var user = await userRepository.GetByEmailAsync(request.Email.Trim());

        // Reject when user does not exist or password hash does not match
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result<TokenResponseDto>.Failure(
                Error.Unauthorized("Auth.InvalidCredentials", AuthMessages.InvalidCredentials));
        }

        // Issue fresh JWT + refresh token pair
        return await IssueTokenPairAsync(user);
    }

    public async Task<Result<TokenResponseDto>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // Find persisted refresh token record
        var existingToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (existingToken is null)
        {
            return Result<TokenResponseDto>.Failure(
                Error.Unauthorized("Auth.InvalidRefreshToken", RefreshTokenMessages.Invalid));
        }

        // Block reuse of revoked tokens (rotation security)
        if (existingToken.IsRevoked)
        {
            return Result<TokenResponseDto>.Failure(
                Error.Unauthorized("Auth.RefreshTokenRevoked", RefreshTokenMessages.Revoked));
        }

        // Block expired refresh tokens
        if (existingToken.Expires <= DateTime.UtcNow)
        {
            return Result<TokenResponseDto>.Failure(
                Error.Unauthorized("Auth.RefreshTokenExpired", RefreshTokenMessages.Expired));
        }

        // Ensure owning user still exists
        var user = await userRepository.GetByIdAsync(existingToken.UserId);
        if (user is null)
        {
            return Result<TokenResponseDto>.Failure(
                Error.Unauthorized("Auth.UserNotFound", AuthMessages.UserNotFound));
        }

        // Revoke old refresh token before issuing a new one (single-use rotation)
        existingToken.IsRevoked = true;
        await refreshTokenRepository.UpdateAsync(existingToken);

        var result = await IssueTokenPairAsync(user);
        await refreshTokenRepository.SaveChangesAsync();
        return result;
    }

    public async Task<Result> LogoutAsync(string refreshToken)
    {
        var existingToken = await refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (existingToken is null)
        {
            return Result.Success();
        }

        existingToken.IsRevoked = true;
        await refreshTokenRepository.UpdateAsync(existingToken);
        await refreshTokenRepository.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<Result<TokenResponseDto>> IssueTokenPairAsync(User user)
    {
        // Generate short-lived JWT access token
        var (accessToken, expiresAt) = jwtTokenGenerator.GenerateToken(user);

        // Generate opaque refresh token value
        var refreshTokenValue = refreshTokenGenerator.GenerateRefreshToken();

        // Persist refresh token with 7-day expiry
        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays)
        };

        await refreshTokenRepository.AddAsync(refreshToken);
        await refreshTokenRepository.SaveChangesAsync();

        return Result<TokenResponseDto>.Success(new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            ExpiresAt = expiresAt
        });
    }
}
