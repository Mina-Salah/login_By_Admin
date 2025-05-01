using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Authorize] // حماية الوصول لجميع الأفعال للمستخدمين المصادق عليهم
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public StudentsController(
            IStudentService studentService,
            SchoolContext context,
            IMapper mapper)
        {
            _studentService = studentService;
            _context = context;
            _mapper = mapper;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return View(students);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.AddStudentAsync(studentViewModel);
                    TempData["Success"] = "Student created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await LoadDropdowns(studentViewModel.CourseId, studentViewModel.TeacherId);
            return View(studentViewModel);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        public async Task<IActionResult> Edit(int id)
        {
            var studentViewModel = await _studentService.GetStudentByIdAsync(id);
            if (studentViewModel == null)
            {
                return NotFound();
            }

            await LoadDropdowns(studentViewModel.CourseId, studentViewModel.TeacherId);
            return View(studentViewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentViewModel studentViewModel)
        {
            if (id != studentViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.UpdateStudentAsync(studentViewModel);
                    TempData["Success"] = "Student updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await LoadDropdowns(studentViewModel.CourseId, studentViewModel.TeacherId);
            return View(studentViewModel);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        public async Task<IActionResult> Delete(int id)
        {
            var studentViewModel = await _studentService.GetStudentByIdAsync(id);
            if (studentViewModel == null)
            {
                return NotFound();
            }

            return View(studentViewModel);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id); // استخدام Soft Delete
                TempData["Success"] = "Student deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


        private async Task LoadDropdowns(int? selectedCourseId = null, int? selectedTeacherId = null)
        {
            var courses = await _context.Courses
                .Where(c => !c.IsDeleted)
                .Select(c => new CourseViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();

            var teachers = await _context.Teachers
                .Where(t => !t.IsDeleted)
                .Select(t => new TeacherViewModel { Id = t.Id, Name = t.Name })
                .ToListAsync();

            ViewBag.Courses = new SelectList(courses, "Id", "Name", selectedCourseId);
            ViewBag.Teachers = new SelectList(teachers, "Id", "Name", selectedTeacherId);
        }
    }
}
