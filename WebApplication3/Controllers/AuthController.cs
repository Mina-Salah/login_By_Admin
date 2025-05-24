using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication3.Models;
using WebApplication3.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;

namespace WebApplication3.Controllers
{

    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Register() => View();
        public IActionResult ChangePassword() => View();
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.Message = "All fields are required.";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Message = "New password and confirmation don't match.";
                return View();
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return RedirectToAction("Login");

            var user = await _authService.GetUserByEmailAsync(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                ViewBag.Message = "Current password is incorrect.";
                return View();
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _authService.UpdateUserAsync(user);

            ViewBag.Message = "Password changed successfully!";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Message = "All fields are required.";
                return View();
            }

            var success = await _authService.RegisterAsync(email, password);

            if (!success)
            {
                ViewBag.Message = "Email already exists.";
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Message = "Email and password are required.";
                return View();
            }

            var user = await _authService.LoginAsync(email, password);

            if (user == null)
            {
                ViewBag.Message = "Invalid email or password.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Welcome");
        }

        [Authorize]
        public IActionResult Welcome()
        {
            ViewBag.Email = User.FindFirst(ClaimTypes.Email)?.Value;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateItem() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateItem(string itemName)
        {
            ViewBag.Message = "Item created successfully!";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteItem() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteItem(int itemId)
        {
            ViewBag.Message = "Item deleted successfully!";
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Message = "Email is required.";
                return View();
            }

            var user = await _authService.GetUserByEmailAsync(email);

            if (user == null)
            {
                ViewBag.Message = "Email not found.";
                return View();
            }

            // توليد كلمة مرور مؤقتة
            string tempPassword = "Temp" + new Random().Next(1000, 9999);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword);

            // تحديث كلمة المرور في قاعدة البيانات
            await _authService.UpdateUserAsync(user);

            // إرسال البريد الإلكتروني بكلمة المرور المؤقتة
            try
            {
                var smtp = new SmtpClient("smtp.yourserver.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your_email@example.com", "your_password"),
                    EnableSsl = true,
                };

                var message = new MailMessage("your_email@example.com", user.Email)
                {
                    Subject = "Password Reset",
                    Body = $"Your temporary password is: {tempPassword}\nPlease change it after logging in."
                };

                await smtp.SendMailAsync(message);

                ViewBag.Message = "Temporary password has been sent to your email.";
            }
            catch
            {
                ViewBag.Message = "Temporary password is: " + tempPassword + " (Email sending failed - configure SMTP)";
            }

            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ViewBag.Message = "Invalid token.";
                return View();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.Message = "All fields are required.";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Message = "New password and confirmation don't match.";
                return View();
            }

            var user = await _authService.GetUserByTokenAsync(token); // هذه الطريقة يجب أن تقوم بإضافة معالج لها
            if (user == null)
            {
                ViewBag.Message = "Invalid token.";
                return View();
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _authService.UpdateUserAsync(user);

            ViewBag.Message = "Password has been reset successfully!";
            return View();
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
