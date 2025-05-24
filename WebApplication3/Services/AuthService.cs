using BCrypt.Net;
using WebApplication3.Models;
using WebApplication3.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;

        public AuthService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == email && u.IsActive);

            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public async Task<bool> RegisterAsync(string email, string password, string role = "User", string? permissions = null)
        {
            var hashed = HashPassword(password);

            var existingUser = (await _userRepository.GetAllAsync())
                .FirstOrDefault(u => u.Email == email);

            if (existingUser != null)
                return false;

            var user = new User
            {
                Email = email,
                PasswordHash = hashed,
                Role = role,
                Permissions = permissions,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
            return true;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var users = await _userRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return (List<User>)await _userRepository.GetAllAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepository.Update(user);
            await _userRepository.SaveAsync();
        }

        public async Task<bool> ToggleUserStatusAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = !user.IsActive;
            _userRepository.Update(user);
            await _userRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateUserPermissionsAsync(int userId, string permissions)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.Role != "Admin") return false;

            user.Permissions = permissions;
            _userRepository.Update(user);
            await _userRepository.SaveAsync();
            return true;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        public async Task<User?> GetUserByTokenAsync(string token)
        {
            var users = await _userRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.UtcNow);
        }
    }
}