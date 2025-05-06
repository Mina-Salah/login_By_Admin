using BCrypt.Net;
using WebApplication3.Models;
using WebApplication3.Repositories;

namespace WebApplication3.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;

        private const string AdminEmail = "admin@example.com";
        private const string AdminPassword = "Admin1234";

        public AuthService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterAsync(string email, string password)
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
                Role = "User"
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
            return true;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            if (email == AdminEmail && password == AdminPassword)
            {
                return new User
                {
                    Email = AdminEmail,
                    Role = "Admin"
                };
            }

            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var users = await _userRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.Email == email);
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

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
        public async Task<User?> GetUserByTokenAsync(string token)
        {
            var users = await _userRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.PasswordHash == token); // استخدم الـ token المرسل لتحديد المستخدم
        }

    }
}
