using BELMS.Application.Interfaces.IService;
using System.Security.Cryptography;

namespace BELMS.Infrastructure.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
