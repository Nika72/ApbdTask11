using System.Threading.Tasks;
using task11.Models;

namespace task11.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        Task<bool> ValidateExpiredTokenAsync(string accessToken);
    }
}