using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication3.Models;
using WebApplication3.Services;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return View();

            var success = await _authService.RegisterAsync(username, password);

            if (!success)
            {
                ViewBag.Message = "Username already exists.";
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _authService.LoginAsync(username, password);

            if (user == null)
            {
                ViewBag.Message = "Invalid username or password.";
                return View();
            }

            // Setup claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role) // "Admin" or "User"
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign in
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Welcome");
        }

        [Authorize]
        public IActionResult Welcome()
        {
            ViewBag.Username = User.Identity.Name;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)?.Value;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateItem(string itemName)
        {
            ViewBag.Message = "Item created successfully!";
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteItem(int itemId)
        {
            ViewBag.Message = "Item deleted successfully!";
            return View();
        }

        // تعديل وظيفة Logout
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // تسجيل الخروج
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // إعادة توجيه المستخدم إلى صفحة تسجيل الدخول
            return RedirectToAction("Login", "Auth");
        }

    }
}
