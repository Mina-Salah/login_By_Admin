using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        // أكشن لصفحة البداية
        public IActionResult Index()
        {
            // لو المستخدم مسجل دخول، حوله مباشرة على صفحة Welcome
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Welcome", "Auth");
            }

            // لو مش مسجل دخول، حوله لصفحة Login
            return RedirectToAction("Login", "Auth");
        }
    }
}
