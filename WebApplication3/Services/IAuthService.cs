using WebApplication3.Models;

namespace WebApplication3.Services
{
    public interface IAuthService
    {
        // User Authentication
        Task<User?> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(string email, string password, string role = "User", string? permissions = null);

        // User Management
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task UpdateUserAsync(User user);
        Task<bool> ToggleUserStatusAsync(int userId);

        // Admin Specific
        Task<bool> UpdateUserPermissionsAsync(int userId, string permissions);

        // Password Operations
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);

        // Token/Reset Operations
        Task<User?> GetUserByTokenAsync(string token);
    }
}
