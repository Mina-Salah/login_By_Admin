using BCrypt.Net;
using Org.BouncyCastle.Crypto.Generators;
using WebApplication3.Models;
using WebApplication3.Repositories;

namespace WebApplication3.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;

        // أدوار ثابتة
        private const string AdminUsername = "Admin";
        private const string AdminPassword = "Admin1412"; // تأكد من استخدام كلمة مرور قوية هنا

        public AuthService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // تسجيل مستخدم جديد
        public async Task<bool> RegisterAsync(string username, string password)
        {
            var hashed = HashPassword(password);

            var existingUser = (await _userRepository.GetAllAsync())
                .FirstOrDefault(u => u.Username == username);

            if (existingUser != null)
                return false; // موجود بالفعل

            // إضافة مستخدم جديد
            var user = new User { Username = username, PasswordHash = hashed, Role = "User" }; // تعيين الدور للمستخدم العادي
            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
            return true;
        }

        // تسجيل دخول المستخدم
        public async Task<User?> LoginAsync(string username, string password)
        {
            // التحقق من "admin" أولاً
            if (username == AdminUsername && password == AdminPassword)
            {
                return new User { Username = AdminUsername, Role = "Admin" }; // إعادة "admin" مباشرة مع تحديد الدور
            }

            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return user; // إذا تم التحقق من كلمة المرور بنجاح
            }

            return null; // إذا كانت بيانات الدخول غير صحيحة
        }

        // تشفير كلمة المرور
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); // استخدام bcrypt لتخزين كلمة المرور
        }

        // التحقق من كلمة المرور
        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash); // التحقق من كلمة المرور باستخدام bcrypt
        }
    }
}
