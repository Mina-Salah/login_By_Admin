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
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly ITeacherService _teacherService;
        private readonly IMapper _mapper;

        public StudentsController(
            IStudentService studentService,
            ICourseService courseService,
            ITeacherService teacherService,
            IMapper mapper)
        {
            _studentService = studentService;
            _courseService = courseService;
            _teacherService = teacherService;
            _mapper = mapper;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return View(students);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        // POST: Students/Create
        [HttpPost]
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
            // استدعاء خدمات الكورسات والمدرسين
            var courses = await _courseService.GetAllCoursesAsync();
            var teachers = await _teacherService.GetAllTeachersAsync();

            // تعبئة الـ ViewBag بالبيانات
            ViewBag.Courses = new SelectList(courses, "Id", "Name", selectedCourseId);
            ViewBag.Teachers = new SelectList(teachers, "Id", "Name", selectedTeacherId);
        }
    }
}
