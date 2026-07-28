using BELMS.Domain.Entities;

namespace BELMS.Application.Interfaces.IService;

public interface IJwtTokenGenerator
{
    (string AccessToken, DateTime ExpiresAt) GenerateToken(User user);
}
