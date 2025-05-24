using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserManagementController : Controller
    {
        private readonly IAuthService _authService;

        public UserManagementController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateAdmin()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(string email, string password, string permissions)
        {
            var success = await _authService.RegisterAsync(email, password, "Admin", permissions);

            if (!success)
            {
                ViewBag.Message = "Failed to create admin user.";
                return View();
            }

            return RedirectToAction("AdminsList");
        }

        public async Task<IActionResult> AdminsList()
        {
            var users = await _authService.GetAllUsersAsync();
            var admins = users.Where(u => u.Role == "Admin").ToList();
            return View(admins);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditAdmin(int id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null || user.Role != "Admin")
                return NotFound();

            return View(user);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> UpdateAdminPermissions(int id, string permissions)
        {
            var success = await _authService.UpdateUserPermissionsAsync(id, permissions);

            if (!success)
            {
                TempData["Message"] = "Failed to update permissions.";
                return RedirectToAction("EditAdmin", new { id });
            }

            TempData["Message"] = "Permissions updated successfully.";
            return RedirectToAction("AdminsList");
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ToggleAdminStatus(int id)
        {
            var success = await _authService.ToggleUserStatusAsync(id);

            if (!success)
            {
                TempData["Message"] = "Failed to change user status.";
            }

            return RedirectToAction("AdminsList");
        }

        public async Task<IActionResult> UsersList()
        {
            var users = await _authService.GetAllUsersAsync();
            var normalUsers = users.Where(u => u.Role == "User").ToList();
            return View(normalUsers);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var success = await _authService.ToggleUserStatusAsync(id);

            if (!success)
            {
                TempData["Message"] = "Failed to change user status.";
            }

            return RedirectToAction("UsersList");
        }
    }
}