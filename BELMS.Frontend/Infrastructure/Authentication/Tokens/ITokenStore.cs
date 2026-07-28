using BELMS.Frontend.Models;

namespace BELMS.Frontend.Infrastructure.Authentication.Tokens
{
    public interface ITokenStore
    {

        Task SaveAsync(string userId,AuthResponse token);
        Task<AuthResponse?> GetAsync(string userId);
        Task RemoveAsync(string userId);

    }
}
