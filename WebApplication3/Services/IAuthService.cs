using WebApplication3.Models;

namespace WebApplication3.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string email, string password);
        Task<User?> LoginAsync(string email, string password);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();
        Task UpdateUserAsync(User user);
        Task<User?> GetUserByTokenAsync(string token);
    }
}
