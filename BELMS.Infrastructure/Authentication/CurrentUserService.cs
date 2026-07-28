using BELMS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BELMS.Infrastructure.Authentication;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var id) ? id : null;
        }
    }

    public string? Email => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public string? Role => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
