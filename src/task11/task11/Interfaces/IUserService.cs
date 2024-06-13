using System.Threading.Tasks;
using task11.Models;

namespace task11.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(User user);
        Task<User> LoginAsync(string username, string password);
        Task<string> GenerateJwtTokenAsync(User user);
        Task<string> GenerateRefreshTokenAsync(User user);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
    }
}